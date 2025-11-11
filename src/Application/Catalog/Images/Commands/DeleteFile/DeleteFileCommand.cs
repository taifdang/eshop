using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Images.Commands.DeleteFile;

public record DeleteFileCommand : IRequest<Unit>
{
    public int ProductId { get; set; }
    public int Id { get; set; }
}

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Unit>
{
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly IFileService _storageService;
    

    public DeleteFileCommandHandler(IRepository<ProductImage> productImageRepository, IFileService storageService)
    {
        _productImageRepository = productImageRepository;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var productImage = await _productImageRepository.FirstOrDefaultAsync(new ImageProductFilterSpec(request.ProductId, request.Id))
                ?? throw new EntityNotFoundException(nameof(ProductImage), request.Id);

        // Remove file
        if(productImage != null)
        {
            if(productImage.ImageUrl != null)
            {
               await _storageService.DeleteFileAsync(new Shared.Models.Media.DeleteFileRequest { FileName = productImage.ImageUrl });
            }
            await _productImageRepository.DeleteAsync(productImage, cancellationToken);
        }
        return Unit.Value;
    }
}

