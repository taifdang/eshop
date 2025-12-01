using Application.Common.Interfaces.Eventbus;
using Application.Common.Interfaces.Persistence;
using Domain.Events;
using MediatR;
using Shared.Constracts.Eventbus.Messages;

namespace Application.Order.EventHandlers;

public class OrderStatusChangedToConfirmedDomainEventHandler : INotificationHandler<OrderStatusChangedToConfirmedDomainEvent>
{
    private readonly IRepository<Domain.Entities.Order> _orderRepository;
    private readonly IEventBus _eventBus;

    public OrderStatusChangedToConfirmedDomainEventHandler(
        IRepository<Domain.Entities.Order> orderRepository, 
        IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(OrderStatusChangedToConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId);
        var orderItems =
            order.Items.Select(x => new OrderStockItem(x.ProductVariantId, x.Quantity)).ToList();

        var integrationEvent = new StockReservationRequested(order.Id, orderItems);
        await _eventBus.SendAsync(integrationEvent, cancellationToken);
    }
}
