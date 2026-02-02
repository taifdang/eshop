using Application.Catalog.Products.Services;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, Unit>
{
    private readonly IProductService _productService;
    private readonly IFileService _storageService;

    public DeleteProductImageCommandHandler(
        IProductService productService, 
        IFileService storageService)
    {
        _productService = productService;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
        Guard.Against.NotFound(request.ProductId, product);

        var productImage = product.Images.FirstOrDefault(x => x.Id == request.Id);
        Guard.Against.NotFound(request.Id, productImage);

        // remove image from storage
        if (productImage.Image != null)
        {
            await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = productImage.Image.FileName });
        }

        product.RemoveImage(request.Id);

        await _productService.UpdateAsync(product, cancellationToken);

        return Unit.Value;
    }
}
