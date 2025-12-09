using Application.Common.Interfaces;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Basket.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;

    public OrderCreatedIntegrationEventHandler(
        IApplicationDbContext dbContext, 
        ILogger<OrderCreatedIntegrationEventHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        var basket = await _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == @event.CustomerId);

        if (basket is not null)
        {
            basket.ClearItems();
            _dbContext.Baskets.Update(basket);
            await _dbContext.SaveChangesAsync();

        }
        else
        {
            _logger.LogWarning("basket empty");
        }
    }
}
