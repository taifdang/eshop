using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TransactionalOutbox.Abstractions;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductOption> ProducOptions => Set<ProductOption>();
    public DbSet<OptionValue> OptionValues => Set<OptionValue>();
    public DbSet<Variant> Variants => Set<Variant>();
    public DbSet<VariantOption> VariantOptions => Set<VariantOption>();
    public DbSet<Basket> Baskets => Set<Basket>();
    public DbSet<BasketItem> BasketItems => Set<BasketItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    public IPollingOutboxMessageRepository OutboxPollingRepository => throw new Exception("OutboxForPollingRepository is not initialized.");

    private IDbContextTransaction _currentTransaction;  

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public async Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
    {
        var stragegy = Database.CreateExecutionStrategy();
        await stragegy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        });
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 1.Use interceptors for Auditable Entities
        // 2.Saving with auditable entities

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // 1.Dispatch Domain Events here.
        // 2.Use interceptors for Dispatching Domain Events

        _ = await SaveChangesAsync(cancellationToken);
        return true;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
    }
}
