using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAcces.Entities;
using System;   
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAcces.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(200);
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Products);
    }
}
