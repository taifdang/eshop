using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.Catalog.Variants.EventHandlers;

public class ReserveStockOnOrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    public ReserveStockOnOrderCreatedEventHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification);
        foreach (var item in notification.Items)
        {
            var variantInStock = await _productVariantRepository
                .FirstOrDefaultAsync(new ProductVariantInStockSpec(item.ProductVariantId), cancellationToken);

            if (variantInStock is null)
            {
                throw new EntityNotFoundException();
            }
            if (item.Quantity > variantInStock.Quantity)
            {
                throw new Exception("Not enough quantity");
            }

            var realStock = variantInStock.Quantity - item.Quantity;

            if (realStock < 0)
            {
                throw new Exception("Stock negative");
            }

            variantInStock.Quantity = realStock;

            await _productVariantRepository.UpdateAsync(variantInStock, cancellationToken);
        }
    }
}