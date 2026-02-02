using Domain.SeedWork;

namespace Domain.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    IUnitOfWork UnitOfWork { get; }
    IQueryable<TEntity> GetQueryableSet();
    Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
    Task<T> SingleOrDefaultAsync<T>(IQueryable<T> query);
    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}
