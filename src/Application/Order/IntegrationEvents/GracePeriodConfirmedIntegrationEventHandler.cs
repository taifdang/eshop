using Application.Common.Interfaces.Eventbus;
using Application.Common.Interfaces.Persistence;
using Application.Order.Commands.SetConfirmedOrderStatus;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Constracts.Eventbus.Messages;

namespace Application.Order.IntegrationEvents;

public class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    IRepository<Domain.Entities.Order> orderRepository,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<GracePeriodConfirmed>
{
    public async Task HandleAsync(GracePeriodConfirmed @event, CancellationToken cancellationToken = default)
    {
        // Business logic: send integration event reduce stock to catalog

        var order = await orderRepository.GetByIdAsync(@event.OrderId, cancellationToken);
        Guard.Against.NotFound(@event.OrderId, order);

        var command = new SetConfirmedOrderStatusCommand(@event.OrderId);

        await mediator.Send(command);

        //logger.LogInformation("Order with Id: {OrderId} has been confirmed. Sending integration event to reserve stock.", order.Id);

        //var orderItems =
        //    order.Items.Select(x => new OrderStockItem(x.ProductVariantId, x.Quantity)).ToList();

        //var eventToSend = new StockReservationRequested(order.Id, orderItems);

        //await eventBus.SendAsync(eventToSend, cancellationToken);
    }
}
