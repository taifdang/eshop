using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.CreateProductImage;

public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileService _storageService;

    public CreateProductImageCommandHandler(IApplicationDbContext dbContext, IFileService storageService)
    {
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        if(request.IsMain)
        {
            var mainImage = await _dbContext.ProductImages
                    .FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.IsMain == true);

            if (mainImage != null)
            {
                throw new Exception("Only one main image");
            }
        }

        var productImage = new ProductImage
        {
            ProductId = request.ProductId,
            IsMain = request.IsMain
        };

        if (request.MediaFile != null)
        {
            var metaData = await _storageService.AddFileAsync(request.MediaFile); // save image with local storage

            productImage.ImageId = Guid.CreateVersion7();
            productImage.Image = new Image
            {
                BaseUrl = metaData.BaseUrl,
                FileName = $"{metaData.Path}/{metaData.Name}",
                AllText = $"seo all text - {request.ProductId}"
            };
        }

        _dbContext.ProductImages.Add(productImage);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
