using Application.Order.Commands.SetProcessingOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;

namespace Application.Order.IntegrationEventHandlers;

public class GracePeriodConfirmedIntegrationEventHandler(IMediator mediator)
    : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    public async Task Handle(GracePeriodConfirmedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetProcessingOrderStatusCommand(integrationEvent.OrderId);

        await mediator.Send(command);
    }
}