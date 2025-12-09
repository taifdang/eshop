using Domain.Events;
using MediatR;

namespace Application.Order.DomainEventHandlers;

public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    public Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        // implement code cancel order, ex: send notification, update stock

        return Task.CompletedTask;
    }
}
