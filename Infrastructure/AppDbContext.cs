using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
#nullable enable
public class AppDbContext : DbContext
{
    private readonly string _connectionString;

    public AppDbContext()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
