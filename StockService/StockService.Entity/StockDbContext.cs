using System;
using Microsoft.EntityFrameworkCore;
using StockService.Entity.Entities;

namespace StockService.Entity
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
              
            modelBuilder.Entity<Stock>(entity =>
            {
                 entity.HasKey(o => o.ProductId);
                 //entity.Property(o => o.Id).ValueGeneratedOnAdd(); 
            });
 
            modelBuilder.Entity<Stock>().HasData(
                new Stock
                { 
                    ProductId = 1,
                    Name = "Beymen Club T-Shirt",
                    Quantity = 98,
                    UnitPrice = 4500m,
                    CreatedAt = new DateTime(2025, 8, 3)
                },
                new Stock
                { 
                    ProductId = 2,
                    Name = "Polo T-Shirt",
                    Quantity = 25,
                    UnitPrice = 2500m,
                    CreatedAt = new DateTime(2025, 4, 5)
                },
                new Stock
                { 
                    ProductId = 3,
                    Name = "New Balance Sneaker",
                    Quantity = 202,
                    UnitPrice = 7500m,
                    CreatedAt = new DateTime(2025, 9, 10)
                }
            ); 
            
        }
    }
}