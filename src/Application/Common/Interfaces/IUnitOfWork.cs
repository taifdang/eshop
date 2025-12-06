namespace Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
