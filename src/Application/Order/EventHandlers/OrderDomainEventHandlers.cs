using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using Domain.Events;
using MediatR;
using System.Text.Json;
using TransactionalOutbox.Abstractions;
using static Contracts.IntegrationEvents.StockReservationRequestedIntegrationEvent;

namespace Application.Order.EventHandlers;

public class OrderDomainEventHandlers 
    : INotificationHandler<OrderStatusChangedToConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPollingOutboxMessageRepository _outboxRepository;

    public OrderDomainEventHandlers(
        IOrderRepository orderRepository, 
        IPollingOutboxMessageRepository outboxRepository)
    {
        _orderRepository = orderRepository;
        _outboxRepository = outboxRepository;
    }

    public async Task Handle(OrderStatusChangedToConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(domainEvent.OrderId);

        Guard.Against.NotFound(domainEvent.OrderId, order);

        var orderItems =
            order.Items.Select(x => new OrderStockItem { VariantId = x.VariantId, Quantity = x.Quantity }).ToList();

        var integrationEvent = new StockReservationRequestedIntegrationEvent
        {
            OrderId = order.Id,
            Items = orderItems
        };

        var message = new PollingOutboxMessage
        {
            CreateDate = DateTime.UtcNow,
            PayloadType = typeof(StockReservationRequestedIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
            Payload = JsonSerializer.Serialize(integrationEvent),
            ProcessedDate = null
        };

        await _outboxRepository.AddAsync(message);
        await _outboxRepository.SaveChangesAsync();
    }
}
