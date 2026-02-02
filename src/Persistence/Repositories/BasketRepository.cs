using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class BasketRepository : Repository<Basket, Guid>, IBasketRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BasketRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Basket?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Basket>()
            .FirstOrDefaultAsync(b => b.CustomerId == customerId, cancellationToken);
    }

    public async Task<Basket?> GetByCustomerIdWithItemsAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Basket>()
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId, cancellationToken);
    }
}
