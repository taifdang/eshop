using Application.Order.Commands.SetStockRejectedOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;

namespace Application.Order.IntegrationEventHandlers;

public class ReserveStockRejectedIntegrationEventHandler(IMediator mediator) 
    : IIntegrationEventHandler<ReserveStockRejectedIntegrationEvent>
{
    public async Task Handle(ReserveStockRejectedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetStockRejectedOrderStatusCommand(integrationEvent.OrderId);

        await mediator.Send(integrationEvent);
    }
}
