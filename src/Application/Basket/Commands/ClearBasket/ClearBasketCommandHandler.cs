using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Basket.Commands.ClearBasket;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public ClearBasketCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefault(b => b.CustomerId == request.CustomerId);

        if (basket is null)
        {
            throw new Exception("Not found basket");
        }

        basket.ClearItems();
        _dbContext.Baskets.Update(basket);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}