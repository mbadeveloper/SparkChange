using Microsoft.EntityFrameworkCore;
using SparkChange.Contracts;
using SparkChange.Utilities;

namespace SparkChange.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>()
                .Property(p => p.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<Product>()
            .HasData(
                new Product
                {
                    Id = 1,
                    Name = "Soup",
                    Unit = ProductUnitValue.Tin,
                    Price = 0.65M,
                    Currency = ApplicationConstants.DefaultCurrency
                },
                new Product
                {
                    Id = 2,
                    Name = "Bread",
                    Unit = ProductUnitValue.Loaf,
                    Price = 0.80M,
                    Currency = ApplicationConstants.DefaultCurrency
                },
                new Product
                {
                    Id = 3,
                    Name = "Milk",
                    Unit = ProductUnitValue.Bottle,
                    Price = 1.15M,
                    Currency = ApplicationConstants.DefaultCurrency
                },
                new Product
                {
                    Id = 4,
                    Name = "Apples",
                    Unit = ProductUnitValue.Bag,
                    Price = 1M,
                    Currency = ApplicationConstants.DefaultCurrency
                });
        }
    }
}
