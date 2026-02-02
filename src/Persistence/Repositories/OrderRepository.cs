using Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class OrderRepository : Repository<Order, Guid>, IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Set<Order>().AddAsync(order);
    }
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<Order>().FindAsync(id);
    }
    public async Task<Order?> GetAsync(Guid id)
    {                  
        return await _dbContext.Set<Order>().Include(m => m.Items).Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Order>?> GetListByCustomerAsync(Guid customerId)
    {
        return await _dbContext.Set<Order>().Where(x => x.CustomerId == customerId).ToListAsync();
    }

    public async Task<Order?> GetByOrderNumber(long orderNumber)
    {
        return await _dbContext.Set<Order>().SingleOrDefaultAsync(x => x.OrderNumber == orderNumber);
    }
}
