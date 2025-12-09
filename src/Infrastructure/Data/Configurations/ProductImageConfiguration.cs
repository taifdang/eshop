using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(nameof(ProductImage));

        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(pi => new { pi.ProductId })
            .IsUnique()
            .HasFilter("[IsMain] = 1"); // Ensures only one main image per product

        // Relationships
        builder.HasOne(pi => pi.Image)
            .WithMany()
            .HasForeignKey(pi => pi.ImageId)
            .OnDelete(DeleteBehavior.Restrict); // EF check constraint
    }
}
