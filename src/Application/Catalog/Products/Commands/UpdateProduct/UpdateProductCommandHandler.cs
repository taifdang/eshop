using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.Id);
        Guard.Against.NotFound(request.Id, product);

        product.Update(name: request.Title, description: request.Description, categoryId: request.CategoryId);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
