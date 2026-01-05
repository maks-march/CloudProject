using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Продукт
        modelBuilder.Entity<Product>().HasKey(n => n.Id);
        modelBuilder.Entity<Product>().Property(n => n.Name).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}