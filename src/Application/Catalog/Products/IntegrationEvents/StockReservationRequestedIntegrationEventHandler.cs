using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Eventbus;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Constracts.Eventbus.Messages;

namespace Application.Catalog.Products.IntegrationEvents;

public class StockReservationRequestedIntegrationEventHandler : IIntegrationEventHandler<StockReservationRequested>
{
    private readonly IRepository<ProductVariant> _variantRepository;
    private readonly IMediator _mediator;

    public StockReservationRequestedIntegrationEventHandler(
        IRepository<ProductVariant> variantRepository, 
        IMediator mediator)
    {
        _variantRepository = variantRepository;
        _mediator = mediator;
    }

    public async Task HandleAsync(StockReservationRequested request, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request);

        var failItems = new List<StockItems>();

        foreach (var item in request.Items)
        {
            var spec = new ProductVariantSpec()
             .ByVariantId(item.VariantId)
             .WithInStock();

            var variant = await _variantRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (variant is null || variant.Quantity < item.Quantity)
            {
                failItems.Add(new StockItems(item.VariantId, item.Quantity, 0));
            }

            else
            {
                variant.DeductStock(item.Quantity);
            }
        }

        if (failItems.Any())
        {
            await _mediator.Publish(new ReserveStockRejectedDomainEvent(request.OrderId, failItems));
            return;
        }

        try
        {
            await _variantRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new ReserveStockSucceededDomainEvent(request.OrderId));
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Concurrency conflict");
        }
    }
}
