using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StarWars.DAL.Entities;

namespace StarWars.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FavoriteCharacter> FavoriteCharacters { get; set; }
    public DbSet<RequestHistory> RequestHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new FavoriteCharacterConfiguration());
        modelBuilder.ApplyConfiguration(new RequestHistoryConfiguration());
    }

    // Add a design-time factory
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=StarWarsDb;Username=postgres;Password=yourpassword");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}