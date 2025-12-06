using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable(nameof(Basket));

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.HasIndex(c => c.CustomerId);

        builder.Property(x => x.Version)
           .IsConcurrencyToken();

        // Relationships
        builder.HasMany(c => c.Items)
           .WithOne()
           .HasForeignKey(c => c.BasketId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
