using Application.Catalog.Products.Services;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            UrlSlug = request.UrlSlug,
            Description = request.Description,
            IsActive = false,
            IsDeleted = false
        };

        await _productService.AddAsync(product, cancellationToken);

        return product.Id;
    }
}