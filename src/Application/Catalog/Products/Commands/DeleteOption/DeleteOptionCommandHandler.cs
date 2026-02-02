using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteOption;

public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, Unit>
{
    private readonly IProductService _productService;

    public DeleteOptionCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        product.RemoveOption(request.OptionId);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}