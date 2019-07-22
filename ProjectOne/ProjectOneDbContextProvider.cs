namespace ProjectOne
{
  using Common;

  using Microsoft.EntityFrameworkCore;

  public class ProjectOneDbContextProvider : IDbContextProvider<ProjectOneDbContext>
  {
    public ProjectOneDbContextProvider()
    {
      this.DBContextOptions = new DbContextOptionsBuilder<ProjectOneDbContext>()
        .UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;").Options;
    }

    public DbContextOptions<ProjectOneDbContext> DBContextOptions { get; set; }

    /// <inheritdoc />
    public ProjectOneDbContext CreateContext()
    {
      return new ProjectOneDbContext(this.DBContextOptions);
    }
  }
}