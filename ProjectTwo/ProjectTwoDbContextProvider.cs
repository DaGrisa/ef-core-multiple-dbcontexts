namespace ProjectOne
{
  using Common;

  using Microsoft.EntityFrameworkCore;

  public class ProjectTwoDbContextProvider : IDbContextProvider<ProjectTwoDbContext>
  {
    public ProjectTwoDbContextProvider()
    {
      this.DBContextOptions = new DbContextOptionsBuilder<ProjectTwoDbContext>()
        .UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;").Options;
    }

    public DbContextOptions<ProjectTwoDbContext> DBContextOptions { get; set; }

    /// <inheritdoc />
    public ProjectTwoDbContext CreateContext()
    {
      return new ProjectTwoDbContext(this.DBContextOptions);
    }
  }
}