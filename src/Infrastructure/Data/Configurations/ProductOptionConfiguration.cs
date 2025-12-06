using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        builder.ToTable(nameof(ProductOption));

        builder.HasKey(po => po.Id);
        builder.Property(po => po.Id)
            .ValueGeneratedOnAdd();

        builder.Property(po => po.ProductId)
            .IsRequired();

        builder.Property(po => po.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.ProductId)
          .IsUnique()
          .HasFilter("[AllowImage] = 1"); // Ensures only one option with images per product

        // Relationships
        builder.HasMany(x => x.Values)
            .WithOne()
            .HasForeignKey(pov => pov.OptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
