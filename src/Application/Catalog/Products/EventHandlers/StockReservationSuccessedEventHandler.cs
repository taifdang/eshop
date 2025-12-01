using Application.Common.Interfaces.Eventbus;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Constracts.Eventbus.Messages;

namespace Application.Catalog.Products.EventHandlers;

public class StockReservationSuccessedEventHandler : INotificationHandler<ReserveStockSucceededDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<StockReservationSuccessedEventHandler> _logger;

    public StockReservationSuccessedEventHandler(
        IEventBus eventBus, 
        ILogger<StockReservationSuccessedEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(ReserveStockSucceededDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = new ReserveStockSucceeded(domainEvent.OrderId);

        await _eventBus.SendAsync(integrationEvent, cancellationToken);
    }
}
