using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public interface IApplicationDbContext : IUnitOfWork, IDisposable
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Image> Images { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<ProductOption> ProducOptions { get; }
    DbSet<OptionValue> OptionValues { get; }
    DbSet<Variant> Variants { get; }
    DbSet<VariantOption> VariantOptions { get; }
    DbSet<Domain.Entities.Basket> Baskets { get; }
    DbSet<BasketItem> BasketItems { get; }
    DbSet<Domain.Entities.Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Domain.Entities.Customer> Customers { get; }
}
