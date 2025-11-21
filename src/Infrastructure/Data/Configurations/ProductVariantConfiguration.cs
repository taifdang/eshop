using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable(nameof(ProductVariant));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(255);

        //builder.Property(x => x.MaxPrice)
        //    .IsRequired()
        //    .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.Percent)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.Sku)
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        // Relationships
        builder.HasOne(x => x.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.VariantOptionValues)
            .WithOne(y => y.ProductVariant)
            .HasForeignKey(vov => vov.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
