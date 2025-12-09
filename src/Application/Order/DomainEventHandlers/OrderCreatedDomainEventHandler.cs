using Contracts.IntegrationEvents;
using Domain.Events;
using MediatR;
using Outbox.Abstractions;
using System.Text.Json;

namespace Application.Order.DomainEventHandlers;

public class OrderCreatedDomainEventHandler(
    IPollingOutboxMessageRepository outboxRepository)
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // logic: order created (integrationEvent) -> clear cart (integrationEventHandler)

        var integrationEvent = new OrderCreatedIntegrationEvent
        {
            OrderId = domainEvent.OrderId,
            CustomerId = domainEvent.CustomerId
        };

        var message = new PollingOutboxMessage
        {
            CreateDate = DateTime.UtcNow,
            PayloadType = typeof(OrderCreatedIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
            Payload = JsonSerializer.Serialize(integrationEvent),
            ProcessedDate = null
        };

        await outboxRepository.AddAsync(message);
        await outboxRepository.SaveChangesAsync();
    }
}
