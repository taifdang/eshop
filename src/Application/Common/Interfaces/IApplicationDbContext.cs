using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<ProductOption> ProductOptions { get; }
    DbSet<OptionValue> OptionValues { get; }
    DbSet<ProductVariant> ProductVariants { get; }
    DbSet<VariantOptionValue> VariantOptionValues { get; }
    DbSet<Domain.Entities.Basket> Baskets { get; }
    DbSet<BasketItem> BasketItems { get; }
    DbSet<Domain.Entities.Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Domain.Entities.Customer> Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
