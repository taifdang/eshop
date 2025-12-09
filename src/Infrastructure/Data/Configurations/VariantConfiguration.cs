using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {
        builder.ToTable(nameof(Variant));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(255);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.Sku)
            .HasMaxLength(100);

        // Relationships
        builder.HasMany(x => x.VariantOptions)
            .WithOne()
            .HasForeignKey(vov => vov.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Variants)
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
