using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartShoppingAssistant.DataAcces.Entities;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAcces.Configuration
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.Reward).IsRequired();
            builder.Property(p => p.RewardValue).IsRequired();

            builder.Property(p => p.Threshold).IsRequired().HasColumnType("decimal(10,2)");

            // Relatii Optionale
            builder.HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .IsRequired(false); // Poate fi null

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(false); // Poate fi null
        }
    }
}