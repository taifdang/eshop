using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using Domain.Events;
using MediatR;
using Outbox.Abstractions;
using System.Text.Json;

namespace Application.Order.DomainEventHandlers;

public class OrderConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    IPollingOutboxMessageRepository outboxRepository)
    : INotificationHandler<OrderConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPollingOutboxMessageRepository _outboxRepository = outboxRepository;

    public async Task Handle(OrderConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // logic: payment succeeded -> set order status comfirmed / paid -> reservation stock (outbox)

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
