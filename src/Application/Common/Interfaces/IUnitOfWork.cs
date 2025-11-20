namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default);
}
