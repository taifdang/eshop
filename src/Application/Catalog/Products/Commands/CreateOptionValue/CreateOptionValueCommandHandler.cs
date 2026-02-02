using Application.Catalog.Products.Services;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public class CreateOptionValueCommandHandler : IRequestHandler<CreateOptionValueCommand, Unit>
{
    private readonly IProductService _productService;
    private readonly IFileService _storageService;

    public CreateOptionValueCommandHandler(
        IProductService productService, 
        IFileService storageService)
    {
        _productService = productService;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(CreateOptionValueCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        var optionValue = new OptionValue
        {
            OptionId = request.OptionId,
            Value = request.Value
        };

        Image? image = null;
        if (request.MediaFile != null)
        {
            var metaData = await _storageService.AddFileAsync(request.MediaFile);

            image = new Image
            {
                Id = Guid.CreateVersion7(),
                BaseUrl = metaData.BaseUrl,
                FileName = $"{metaData.Path}/{metaData.Name}",
                AllText = $"seo all text - {request.ProductId}"
            };
        }

        product.AddOptionValue(request.OptionId, optionValue, image);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}

