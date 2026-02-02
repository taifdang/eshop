using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories;

public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }
}
