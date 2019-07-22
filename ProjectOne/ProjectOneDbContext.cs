namespace ProjectOne
{
  using Microsoft.EntityFrameworkCore;

  public class ProjectOneDbContext : DbContext
  {
    /// <inheritdoc />
    public ProjectOneDbContext(DbContextOptions options)
      : base(options)
    {
    }

    /// <inheritdoc />
    public ProjectOneDbContext()
    {
    }

    public virtual DbSet<ClassOne> ClassOne { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;");

      base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("projectOne");
      base.OnModelCreating(modelBuilder);
    }
  }
}