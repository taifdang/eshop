using Application.Order.Commands.SetConfirmedOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;

namespace Application.Order.IntegrationEventHandlers;

public class PaymentSucceededIntegrationEventHandler(IMediator mediator) 
    : IIntegrationEventHandler<PaymentSucceededIntegrationEvent>
{
    public async Task Handle(PaymentSucceededIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetConfirmedOrderStatusCommand(integrationEvent.OrderNumber, integrationEvent.CardBrand, integrationEvent.TransactionId);

        await mediator.Send(command);
    }
}
