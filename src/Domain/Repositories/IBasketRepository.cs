using Domain.Entities;

namespace Domain.Repositories;

public interface IBasketRepository : IRepository<Basket, Guid>
{
    Task<Basket?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Basket?> GetByCustomerIdWithItemsAsync(Guid customerId, CancellationToken cancellationToken = default);
}
