namespace Application.Common.Interfaces;

public interface IOrderRepository : IRepository<Domain.Entities.Order>
{
    Task<Domain.Entities.Order?> GetByIdAsync(Guid id);
    Task<Domain.Entities.Order?> GetAsync(Guid id);
    Task<List<Domain.Entities.Order>?> GetListAsync();
    Task<Domain.Entities.Order?> GetByOrderNumber(long orderNumber);
    Task AddAsync(Domain.Entities.Order order);
}
