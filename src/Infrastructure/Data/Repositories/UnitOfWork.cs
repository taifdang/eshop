using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
    public async Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
    {
        var stragegy = _dbContext.Database.CreateExecutionStrategy();
        await stragegy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
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
        });
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
