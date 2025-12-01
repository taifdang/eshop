using Domain.Entities;
using Domain.Enums;
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
            .HasConversion<string>() // Store enum as string in database
            .HasMaxLength(20);

        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        //builder.Property(o => o.Address)
        //    .IsRequired()
        //    .HasMaxLength(500);

        builder.OwnsOne(
            x => x.ShippingAddress,
            a =>
            {
                //a.Property(p => p.FullName)
                // .HasColumnName(nameof(Address.FullName))
                // .HasMaxLength(128)
                // .IsRequired();

                //a.Property(p => p.PhoneNumber)
                // .HasColumnName(nameof(Address.PhoneNumber))
                // .HasMaxLength(12)
                // .IsRequired();

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

        builder.OwnsOne(
            x => x.Payment,
            a =>
            {
                a.Property(p => p.TransactionId)
                 .HasColumnName(nameof(Payment.TransactionId));

                a.Property(p => p.Provider)
                 .HasColumnName(nameof(Payment.Provider))
                 .HasDefaultValue(PaymentProvider.None)
                 .HasConversion(
                    x => x.ToString(),
                    x => (PaymentProvider)Enum.Parse(typeof(PaymentProvider), x));

                a.Property(p => p.Amount)
                 .HasColumnType("decimal(18,2)")
                 .IsRequired();

                a.Property(p => p.Method)
                .HasColumnName(nameof(Payment.Method))
                .HasDefaultValue(PaymentMethod.None)
                .HasConversion(
                   x => x.ToString(),
                   x => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), x));

                a.Property(p => p.Status)
                 .HasColumnName(nameof(Payment.Status))
                 .HasDefaultValue(PaymentStatus.Pending)
                 .HasConversion(
                   x => x.ToString(),
                   x => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), x));

                a.Property(p => p.PaymentUrl)
                 .HasColumnName(nameof(Payment.PaymentUrl));

                a.Property(p => p.PaidAt)
                 .HasColumnName(nameof(Payment.PaidAt));
            });

        builder.Property(o => o.OrderDate)
            .IsRequired();

        builder.Property(r => r.Version).IsConcurrencyToken();

        // Configure one-to-many relationship with OrderItem
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)  // Shadow property for foreign key
            .OnDelete(DeleteBehavior.Cascade); // Delete items when order is deleted

        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrderDate);
    }
}
