using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAcces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.DataAcces.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItem");
        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.Quantity).IsRequired();
        builder.HasOne(ci => ci.Product)
               .WithMany()
               .HasForeignKey(ci => ci.ProductId);
    }
}
