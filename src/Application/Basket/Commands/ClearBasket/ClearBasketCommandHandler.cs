using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Basket.Commands.ClearBasket;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public ClearBasketCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == request.CustomerId);

        if (basket != null)
        {
            basket.ClearItems();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}