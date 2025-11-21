using Application.Common.Interfaces;
using Application.Common.Specifications;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Images.Commands.DeleteFile;

public record DeleteFileCommand(Guid ProductId, Guid Id) : IRequest<Unit>;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Unit>
{
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly IFileService _storageService;

    public DeleteFileCommandHandler(
        IFileService storageService,
        IRepository<ProductImage> productImageRepository)
    {
        _storageService = storageService;
        _productImageRepository = productImageRepository;
    }

    public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var spec = new ProductImageByIdSpec(request.ProductId, request.Id);
        var productImage = await _productImageRepository.FirstOrDefaultAsync(spec, cancellationToken);
        Guard.Against.Null(productImage);

        // Remove file
        if (productImage != null)
        {
            if(productImage.ImageUrl != null)
            {
               await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = productImage.ImageUrl });
            }
            await _productImageRepository.DeleteAsync(productImage, cancellationToken);
        }

        return Unit.Value;
    }
}

