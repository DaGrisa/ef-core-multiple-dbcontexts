namespace WebAppStartup
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Runtime.Loader;

  using Autofac;

  using Common;

  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;

  public class Startup
  {
    private IContainer Container;

    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      this.LoadAssemblies();

      this.Migrate();
    }

    private IEnumerable<Type> GetClassesImplementingAnInterface(Assembly assemblyToScan, Type implementedInterface)
    {
      if (assemblyToScan.FullName.ToLower().Contains("projectone")) {
        var debug = "here";
      }

      if (assemblyToScan == null)
        return new List<Type>();

      if (implementedInterface == null)
        return new List<Type>();

      IEnumerable<Type> typesInTheAssembly;

      try {
        typesInTheAssembly = assemblyToScan.GetTypes();
      }
      catch (ReflectionTypeLoadException e) {
        typesInTheAssembly = e.Types.Where(t => t != null);
      }

      IList<Type> classesImplementingInterface = new List<Type>();

      // if the interface is a generic interface
      if (implementedInterface.IsGenericType) {
        foreach (var typeInTheAssembly in typesInTheAssembly) {
          if (typeInTheAssembly.IsClass && !typeInTheAssembly.IsAbstract) {
            var typeInterfaces = typeInTheAssembly.GetInterfaces();
            foreach (var typeInterface in typeInterfaces) {
              if (typeInterface.IsGenericType) {
                var typeGenericInterface = typeInterface.GetGenericTypeDefinition();
                var implementedGenericInterface = implementedInterface.GetGenericTypeDefinition();

                if (typeGenericInterface == implementedGenericInterface)
                  classesImplementingInterface.Add(typeInTheAssembly);
              }
            }
          }
        }
      } else {
        foreach (var typeInTheAssembly in typesInTheAssembly) {
          if (typeInTheAssembly.IsClass && !typeInTheAssembly.IsAbstract) {
            // if the interface is a non-generic interface
            if (implementedInterface.IsAssignableFrom(typeInTheAssembly))
              classesImplementingInterface.Add(typeInTheAssembly);
          }
        }
      }

      return classesImplementingInterface;
    }

    private IEnumerable<Type> GetClassesWhichImplementInterface(
      Type implementedInterface,
      IEnumerable<Assembly> assemblies)
    {
      var types = new List<Type>();
      foreach (var assembly in assemblies) {
        var classes = this.GetClassesImplementingAnInterface(assembly, implementedInterface);
        types.AddRange(classes);
      }

      return types;
    }

    private void LoadAssemblies()
    {
      var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var files = Directory.GetFiles(location, "*.dll");

      foreach (var file in files) {
        var assemblyName = AssemblyName.GetAssemblyName(file);
        if (assemblyName.Name.ToLower().StartsWith("project")) {
          var isFound = (from a in loadedAssemblies where a.GetName().Name == assemblyName.Name select a).Any();
          if (isFound) {
            continue;
          }

          AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
        }
      }

      loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

      var builder = new ContainerBuilder();

      builder.RegisterAssemblyTypes(loadedAssemblies)
        .Where(t => typeof(IDbContextProvider<>).IsAssignableFrom(t) && !t.IsAbstract);
      builder.RegisterAssemblyTypes(loadedAssemblies)
        .Where(t => typeof(DbContext).IsAssignableFrom(t) && !t.IsAbstract);

      this.Container = builder.Build();
    }

    private void Migrate()
    {
      // Get all loaded assemblies and look for entity framework database context files
      var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      var contextProviders = this.GetClassesWhichImplementInterface(typeof(IDbContextProvider<>), loadedAssemblies);
      var container = this.Container;

      foreach (var dbContextProvider in contextProviders) {
        var dbContextType = (from ifc in dbContextProvider.GetInterfaces()
                             where ifc.GetGenericTypeDefinition() == typeof(IDbContextProvider<>)
                                   && ifc.GetGenericArguments() != null && ifc.GetGenericArguments().Length > 0
                             select ifc.GetGenericArguments()[0]).FirstOrDefault();

        if (container.Resolve(dbContextType) is DbContext dbContext && dbContext.Database.IsSqlServer()) {
          dbContext.Database.MigrateAsync();
        }
      }
    }
  }
}