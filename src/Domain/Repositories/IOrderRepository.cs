using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order?> GetAsync(Guid id);
    Task<List<Order>?> GetListByCustomerAsync(Guid customerId);
    Task<Order?> GetByOrderNumber(long orderNumber);
    Task AddAsync(Order order);
}
