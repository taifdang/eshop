using Application.Order.Commands.SetCompletedOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;

namespace Application.Order.IntegrationEventHandlers;

public class ReserveStockSucceededIntegrationEventHandler(IMediator mediator) : IIntegrationEventHandler<ReserveStockSucceededIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;
    public async Task Handle(ReserveStockSucceededIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetCompletedOrderStatusCommand(integrationEvent.OrderId);

        await _mediator.Send(command);
    }
}
