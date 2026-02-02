using Domain.Repositories;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    private readonly ApplicationDbContext _dbContext;
    public ReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
    public IQueryable<TEntity> GetQueryableSet()
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.FirstOrDefaultAsync();
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query)
    {
        return query.SingleOrDefaultAsync();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return query.ToListAsync();
    }
}
