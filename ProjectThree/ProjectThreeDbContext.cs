namespace ProjectOne
{
  using Microsoft.EntityFrameworkCore;

  using ProjectTwo;

  public class ProjectThreeDbContext : DbContext
  {
    /// <inheritdoc />
    public ProjectThreeDbContext(DbContextOptions options)
      : base(options)
    {
    }

    /// <inheritdoc />
    public ProjectThreeDbContext()
    {
    }

    public virtual DbSet<ClassThree> ClassThree { get; set; }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Data Source=.\\MSSQL2016;Initial Catalog=WebApp;Integrated Security=SSPI;");

      base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("projectThree");
      base.OnModelCreating(modelBuilder);
    }
  }
}