namespace Application.Common.Interfaces;

public interface IOrderRepository : IRepository<Domain.Entities.Order>
{
    Task<Domain.Entities.Order?> GetAsync(Guid id);
    Task AddAsync(Domain.Entities.Order order);
}
