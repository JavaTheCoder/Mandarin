using Mandarin.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mandarin.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .HasData(
                    new Category { Id = 1, Name = "Laptops" },
                    new Category { Id = 2, Name = "Computers" }
                );

            modelBuilder.Entity<Product>()
                .HasData(
                    new Product
                    {
                        Id = 1,
                        CategoryId = 1,
                        Name = "Acer ProBook",
                        Price = 599,
                        Image = "https://cdn.thewirecutter.com/wp-content/media/2022/07/laptop-under-500-2048px-acer-1.jpg",
                        Description = "An overpriced ultrabook with terrible specs",
                        UserId = "9cde0a18-1721-474c-ba84-804e7a88888b"
                    },
                    new Product
                    {
                        Id = 2,
                        CategoryId = 1,
                        Name = "Dell Alienware",
                        Price = 2499,
                        Image = "https://i.dell.com/sites/csimages/Product_Imagery/all/fp-aw-laptops-hero-a-1920x1440-v2.png",
                        Description = "An overpriced gaming laptop with promising specs",
                        UserId = "9cde0a18-1721-474c-ba84-804e7a88888b"
                    }
                );
        }
    }
}