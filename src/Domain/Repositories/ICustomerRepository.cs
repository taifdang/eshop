using Domain.Entities;

namespace Domain.Repositories;

public interface ICustomerRepository : IRepository<Customer, Guid>
{
}
