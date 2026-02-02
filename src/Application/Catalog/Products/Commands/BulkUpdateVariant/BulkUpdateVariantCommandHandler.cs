using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.BulkUpdateVariant;

public class BulkUpdateVariantCommandHandler : IRequestHandler<BulkUpdateVariantCommand, Guid>
{
    private readonly IProductService _productService;

    public BulkUpdateVariantCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Guid> Handle(BulkUpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        product.BulkUpdateVariants(
            price: request.Price,
            quantity: request.Quantity,
            sku: request.Sku,
            isActive: request.IsActive
        );

        await _productService.UpdateAsync(product, cancellationToken);

        return request.ProductId;
    }
}
