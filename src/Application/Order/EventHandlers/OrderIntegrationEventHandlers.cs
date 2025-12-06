using Application.Common.Interfaces;
using Application.Order.Commands.SetConfirmedOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using EventBus.Events;
using MediatR;

namespace Application.Order.EventHandlers;

public class OrderIntegrationEventHandlers(
    IMediator mediator,
    IOrderRepository orderRepository) : 
    IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>,
    IIntegrationEventHandler<OrderCompletedIntegrationEvent>
{
    public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
    {
        var order = await orderRepository.GetAsync(@event.OrderId);

        Guard.Against.NotFound(@event.OrderId, order);

        var command = new SetConfirmedOrderStatusCommand(@event.OrderId);

        await mediator.Send(command);
    }

    public Task Handle(OrderCompletedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }

    public Task Handle(IntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
