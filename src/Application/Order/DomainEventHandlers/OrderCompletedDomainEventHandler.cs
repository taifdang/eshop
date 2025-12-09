using Domain.Events;
using MediatR;

namespace Application.Order.DomainEventHandlers;

public class OrderCompletedDomainEventHandler : INotificationHandler<OrderCompletedDomainEvent>
{
    public Task Handle(OrderCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // implement code cancel order, ex: send notification

        return Task.CompletedTask;
    }
}
