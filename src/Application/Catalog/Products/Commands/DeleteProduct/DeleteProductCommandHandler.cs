using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductService _productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.Id);
        Guard.Against.NotFound(request.Id, product);

        product.MarkAsDeleted();

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}