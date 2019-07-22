namespace ProjectOne
{
  using Microsoft.EntityFrameworkCore;

  using ProjectTwo;

  public class ProjectTwoDbContext : DbContext
  {
    /// <inheritdoc />
    public ProjectTwoDbContext(DbContextOptions options)
      : base(options)
    {
    }

    /// <inheritdoc />
    public ProjectTwoDbContext()
    {
    }

    public virtual DbSet<ClassTwo> ClassTwo { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;");

      base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("projectTwo");
      base.OnModelCreating(modelBuilder);
    }
  }
}