namespace ProjectOne
{
  using Common;

  using Microsoft.EntityFrameworkCore;

  public class ProjectOneDbContextProvider : IDbContextProvider<ProjectThreeDbContext>
  {
    public ProjectOneDbContextProvider()
    {
      this.DBContextOptions = new DbContextOptionsBuilder<ProjectThreeDbContext>()
        .UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;").Options;
    }

    public DbContextOptions<ProjectThreeDbContext> DBContextOptions { get; set; }

    /// <inheritdoc />
    public ProjectThreeDbContext CreateContext()
    {
      return new ProjectThreeDbContext(this.DBContextOptions);
    }
  }
}