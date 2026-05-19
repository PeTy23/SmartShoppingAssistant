using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess;

public class SmartShoppingAssistantDbContext(DbContextOptions<SmartShoppingAssistantDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartShoppingAssistantDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Pasta & Grains", Description = "Dried pasta, rice, and grain products" },
            new Category { Id = 2, Name = "Dairy & Eggs", Description = "Milk, cheese, yogurt, and eggs" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Spaghetti", Description = "Classic Italian spaghetti, 500g", Price = 5.99m, ImageUrl = "/images/spaghetti.jpg" },
            new Product { Id = 2, Name = "Parmesan Cheese", Description = "Aged parmesan, 200g block", Price = 12.50m, ImageUrl = "/images/parmesan.jpg" },
            new Product { Id = 3, Name = "Tomato Sauce", Description = "Organic tomato basil sauce", Price = 8.99m, ImageUrl = "/images/sauce.jpg" }
        );

        modelBuilder.Entity("CategoryProduct").HasData(
            new { CategoriesId = 1, ProductsId = 1 }, // Spaghetti aparține de Pasta
            new { CategoriesId = 2, ProductsId = 2 }, // Parmesan aparține de Dairy
            new { CategoriesId = 1, ProductsId = 3 }  // Sosul de roșii aparține tot de Pasta
        );

        modelBuilder.Entity<Promotion>().HasData(
            new Promotion
            {
                Id = 1,
                Name = "Buy 5 Get 1 Free Spaghetti",
                Type = Entities.Enums.PromotionType.Quantity,
                Threshold = 5,
                Reward = Entities.Enums.PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId = 1,
                IsActive = true
            },
            new Promotion
            {
                Id = 2,
                Name = "10% off orders over 100 RON",
                Type = Entities.Enums.PromotionType.CartTotal,
                Threshold = 100.00m,
                Reward = Entities.Enums.PromotionReward.PercentDiscount,
                RewardValue = 10,
                IsActive = true
            }
        );
    }
}