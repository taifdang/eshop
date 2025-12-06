using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        // Composite primary key (OrderId + VariantId)
        builder.HasKey(oi => new { oi.OrderId, oi.VariantId });

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.VariantId)
            .IsRequired();

        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(oi => oi.UnitPrice)
          .IsRequired()
          .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.HasIndex(oi => oi.VariantId);
        builder.HasIndex(oi => oi.OrderId);

        // Configure relationship with Order (optional, but explicit)
        //builder.HasOne<Order>()
        //    .WithMany(o => o.Items)
        //    .HasForeignKey(oi => oi.OrderId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
