using Application.Catalog.Products.Services;
using MediatR;

namespace Application.Catalog.Products.Commands.ActiveProduct;

public class ActiveProductCommandHandler : IRequestHandler<ActiveProductCommand, Unit>
{
    private readonly IProductService _productService;

    public ActiveProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(ActiveProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {request.ProductId} not found.");
        }

        if (request.IsActive)
        {
            product.Activate();
        }
        else
        {
            product.Deactivate();
        }

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
