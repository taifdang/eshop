using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

//ref: https://www.codesimple.dev/blogPost/ea8c4560-36a5-4d72-9ed6-5f6d74a22036
//ref: https://code-maze.com/cqrs-mediatr-fluentvalidation
namespace Application.Catalog.Images.Commands.UploadFile;

public record UploadFileCommand : IRequest<Unit>
{
    public int ProductId { get; init; }
    public int? OptionValueId { get; init; }
    public IFormFile MediaFile { get; init; }
    public bool IsMain { get; init; } = false;
}

public class CreateImageCommandHandler : IRequestHandler<UploadFileCommand, Unit>
{
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly IRepository<OptionValue> _optionValueRepository;
    private readonly IFileService _storageService;
    public CreateImageCommandHandler(
        IRepository<ProductImage> productImageRepository,
        IRepository<OptionValue> optionValueRepository,
        IFileService storageService)
    {
        _productImageRepository = productImageRepository;
        _optionValueRepository = optionValueRepository;
        _storageService = storageService;    
    }
    public async Task<Unit> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        // Validate
        //if (request.MediaFile is null)
        //    throw new ArgumentException("Media file cannot be null.", nameof(request.MediaFile));

        if (request.OptionValueId.HasValue)
        {       
            if(!await _optionValueRepository.AnyAsync(new OptionValueWithImageSpec(request.OptionValueId.Value, request.ProductId)))
            {
                throw new EntityNotFoundException(nameof(OptionValue), "This OptionValue not support add image");
            }
        }
        // Save file
        if (request.MediaFile != null)
        {
            var pathMedia = await _storageService.AddFileAsync(request.MediaFile);
            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                OptionValueId = request.OptionValueId,
                ImageUrl = pathMedia.Path,
                IsMain = request.OptionValueId.HasValue ? true : request.IsMain
            };
            await _productImageRepository.AddAsync(productImage, cancellationToken);
        }
        return Unit.Value;
    }
}
