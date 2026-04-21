using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAcces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAcces.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasMany(c => c.Products)
                .WithMany(p => p.Categories);
        }
    }
}
