using Application.Common.Interfaces;
using Infrastructure.Identity.Data;

namespace Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppIdentityDbContext _context;

    public UnitOfWork(AppIdentityDbContext context)
    {
        _context = context;
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            await action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
        }
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
