using Application.Common.Interfaces;
using Application.Order.Commands.SetConfirmedOrderStatus;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Order.IntegrationEvents;

public class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    IApplicationDbContext _dbContext,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
    {
        // Business logic: send integration event reduce stock to catalog
        var order = await _dbContext.Orders.FindAsync(@event.OrderId);

        Guard.Against.NotFound(@event.OrderId, order);

        var command = new SetConfirmedOrderStatusCommand(@event.OrderId);

        await mediator.Send(command);
    }
}
