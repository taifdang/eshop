using Domain.Entities;
using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>(); // Store enum as string in database

        builder.OwnsOne(
           x => x.TotalAmount,
           a =>
           {
               a.Property(p => p.Amount)
                .HasColumnName(nameof(Money.Amount))
                .HasColumnType("decimal(18,2)")
                .IsRequired();

               a.Property(p => p.Currency)
                .HasColumnName(nameof(Money.Currency))
                .HasMaxLength(10)
                .IsRequired();
           });

        builder.OwnsOne(
            x => x.ShippingAddress,
            a =>
            {
                a.Property(p => p.Street)
                 .HasColumnName(nameof(Address.Street))
                 .HasMaxLength(256)
                 .IsRequired();

                a.Property(p => p.City)
                 .HasColumnName(nameof(Address.City))
                 .HasMaxLength(100)
                 .IsRequired();

                a.Property(p => p.ZipCode)
                 .HasColumnName(nameof(Address.ZipCode))
                 .HasMaxLength(10)
                 .IsRequired();
            });      

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(r => r.Version).IsConcurrencyToken();

        // Configure one-to-many relationship with OrderItem: Unidirectional
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)  // Shadow property for foreign key
            .OnDelete(DeleteBehavior.Cascade); // Delete items when order is deleted

        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrderDate);
    }
}
