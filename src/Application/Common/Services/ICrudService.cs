using Domain.SeedWork;

namespace Application.Common.Services;

public interface ICrudService<T> where T : Entity<Guid>
{
    Task<List<T>> GetAsync(CancellationToken cancellationToken = default);

    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
