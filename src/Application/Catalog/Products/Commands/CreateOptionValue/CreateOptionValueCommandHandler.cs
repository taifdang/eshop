using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public class CreateOptionValueCommandHandler : IRequestHandler<CreateOptionValueCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileService _storageService;

    public CreateOptionValueCommandHandler(
        IApplicationDbContext dbContext, 
        IFileService storageService)
    {
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<Unit> Handle(CreateOptionValueCommand request, CancellationToken cancellationToken)
    {
        // check productOption allow image
        var productOption = await _dbContext.ProducOptions
            .SingleOrDefaultAsync(m => m.Id == request.OptionId && m.AllowImage, cancellationToken);

        // optionValue > 1 image (optional)
        if(productOption == null)
        {
            throw new Exception("Product option does not exist or does not allow images.");
        }

        var optionValue = new OptionValue
        {
            OptionId = request.OptionId,
            Value = request.Value
        };

        if (request.MediaFile != null)
        {
            var metaData = await _storageService.AddFileAsync(request.MediaFile); // save image with local storage

            optionValue.ImageId = Guid.CreateVersion7();
            optionValue.Image = new Image
            {
                BaseUrl = metaData.BaseUrl,
                FileName = $"{metaData.Path}/{metaData.Name}",
                AllText = $"seo all text - {productOption.ProductId}"
            };   
        }

        _dbContext.OptionValues.Add(optionValue);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

