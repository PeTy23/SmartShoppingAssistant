using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAcces.Configuration;
using SmartShoppingAssistant.DataAcces.Entities;
using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAcces;

public class SmartShoppingDbContext(DbContextOptions<SmartShoppingDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null;

    public DbSet<Category> Categories { get; set; } = null;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartShoppingDbContext).Assembly);
    }
}
