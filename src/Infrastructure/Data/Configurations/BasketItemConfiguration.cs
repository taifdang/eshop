using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable(nameof(BasketItem));

        builder.HasKey(ci => new { ci.BasketId, ci.VariantId });

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.HasIndex(ci => ci.BasketId);
        builder.HasIndex(ci => ci.VariantId);

        // Relationships
        //builder.HasOne<Basket>()
        //    .WithMany(c => c.Items)
        //    .HasForeignKey(ci => ci.BasketId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
