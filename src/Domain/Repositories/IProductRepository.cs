using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepository : IRepository<Product, Guid>
{
}
