using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Basket.Commands.ClearBasket;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<ClearBasketCommandHandler> _logger;

    public ClearBasketCommandHandler(
        IApplicationDbContext dbContext, 
        ILogger<ClearBasketCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefault(b => b.CustomerId == request.CustomerId);

        if (basket != null)
        {
            basket.ClearItems();
            _dbContext.Baskets.Update(basket);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}