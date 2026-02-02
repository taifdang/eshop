using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.UpdateVariant;

public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, Unit>
{
    private readonly IProductService _productService;

    public UpdateVariantCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        product.UpdateVariant(request.Id, price: request.RegularPrice, quantity: request.Quantity);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
