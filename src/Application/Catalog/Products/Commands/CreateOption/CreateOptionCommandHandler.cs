using Application.Catalog.Products.Services;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateOption;

public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, Unit>
{
    private readonly IProductService _productService;

    public CreateOptionCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Unit> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {request.ProductId} not found.");
        }

        var productOption = new ProductOption
        {
            ProductId = request.ProductId,
            Name = request.OptionName,
            AllowImage = request.AllowImage,
        };

        product.AddOption(productOption);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
