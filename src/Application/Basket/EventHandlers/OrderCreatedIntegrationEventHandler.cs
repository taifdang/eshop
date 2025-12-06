using Application.Common.Interfaces;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Application.Basket.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IApplicationDbContext _dbContext;

    public OrderCreatedIntegrationEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        var basket = await _dbContext.Baskets
            .Include(m => m.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == @event.CustomerId);

        if (basket != null)
        {
            basket.ClearItems();
            await _dbContext.SaveChangesAsync();
        }
    }
}
