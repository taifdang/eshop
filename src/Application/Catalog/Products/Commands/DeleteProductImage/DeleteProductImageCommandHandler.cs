using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileService _storageService;

    public DeleteProductImageCommandHandler(
        IApplicationDbContext dbContext, 
        IFileService storageService)
    {
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var productImage = await _dbContext.ProductImages
               .FirstOrDefaultAsync(x => x.Id == request.Id && x.ProductId == request.ProductId);

        Guard.Against.NotFound(request.Id, productImage);

        // remove image
        if (productImage.Image != null)
        {
            await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = productImage.Image.FileName });
        }

        _dbContext.ProductImages.Remove(productImage);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
