using Mandarin.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mandarin.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FavoriteProduct> Favorites { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Chat>()
                .HasData(
                    new Chat
                    {
                        Id = 1,
                        OwnerName = "Barry",
                        CustomerName = "JV",
                        ProductId = 2
                    }
                );

            modelBuilder.Entity<Message>()
                .HasData(
                    new Message
                    {
                        Id = 1,
                        Text = "hi Barry. I wanna buy this laptop",
                        From = "JV",
                        To = "Barry",
                        ChatId = 1,
                        Date = DateTime.Now.AddMinutes(-30)
                    },
                    new Message
                    {
                        Id = 2,
                        Text = "hi JV, you sure you can afford it?",
                        From = "Barry",
                        To = "JV",
                        ChatId = 1,
                        Date = DateTime.Now
                    }
                );

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

            base.OnModelCreating(modelBuilder);
        }
    }
}