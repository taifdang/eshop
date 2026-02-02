using Contracts.IntegrationEvents;
using Domain.Repositories;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Basket.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IBasketRepository _repository;
    private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;

    public OrderCreatedIntegrationEventHandler(
        IBasketRepository repository,
        ILogger<OrderCreatedIntegrationEventHandler> logger
        )
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        var basket = await _repository.GetByCustomerIdWithItemsAsync(@event.CustomerId);

        if (basket is not null)
        {
            basket.ClearItems();
            await _repository.UnitOfWork.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("basket empty");
        }
    }
}
