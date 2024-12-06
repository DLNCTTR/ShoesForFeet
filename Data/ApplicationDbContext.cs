using Microsoft.EntityFrameworkCore;
using ShoesForFeet.Models;

namespace ShoesForFeet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Running Shoes",
                    ShoeSize = 10,
                    Price = 79.99m,
                    ImageUrl = "/Images/Products/Runners.jpg",
                    Description = "Comfortable running shoes designed for performance."
                },
                new Product
                {
                    Id = 2,
                    Name = "Casual Sneakers",
                    ShoeSize = 9,
                    Price = 49.99m,
                    ImageUrl = "/Images/Products/DressShoes.jpg",
                    Description = "Classy shoes for any event."
                },
                new Product
                {
                    Id = 3,
                    Name = "Boots",
                    ShoeSize = 14,
                    Price = 79.99m,
                    ImageUrl = "/Images/Products/Boots.jpg",
                    Description = "Work boots suitable for any terrain."
                }
            );
        }
    }
}