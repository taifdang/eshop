using Application.Catalog.Products.Services;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateProductImage;

public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, Unit>
{
    private readonly IProductService _productService;
    private readonly IFileService _storageService;

    public CreateProductImageCommandHandler(IProductService productService, IFileService storageService)
    {
        _productService = productService;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        var productImage = new ProductImage
        {
            ProductId = request.ProductId,
            IsMain = request.IsMain
        };

        if (request.MediaFile != null)
        {
            var metaData = await _storageService.AddFileAsync(request.MediaFile);

            productImage.ImageId = Guid.CreateVersion7();
            productImage.Image = new Image
            {
                BaseUrl = metaData.BaseUrl,
                FileName = $"{metaData.Path}/{metaData.Name}",
                AllText = $"seo all text - {request.ProductId}"
            };
        }

        product.AddImage(productImage);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
