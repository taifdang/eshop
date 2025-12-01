using Application.Catalog.Products.Commands.UploadFile;
using Application.Catalog.Products.Queries.GetProductImage;
using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Application.Common.Models;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Catalog.Products.Services;

public class ProductImageService : IProductImageService
{
    private readonly IRepository<ProductImage> _productImageRepo;
    private readonly IRepository<Product> _productRepo;

    public ProductImageService(
        IRepository<ProductImage> repo, 
        IRepository<Product> productRepo)
    {
        _productImageRepo = repo;
        _productRepo = productRepo;
    }

    public async Task<ProductImageResult> GetOrderedImagesAsync(Guid productId, Guid? optionValueId = null, CancellationToken ct = default)
    {
        var spec = new ProductImageSpec()
            .ByProductId(productId)
            .ApplyOrderingBy(optionValueId)
            .WithProjectionOf(new ProductImageProjectionSpec());

        var imagesWithMeta = await _productImageRepo.ListAsync(spec, ct);

        if (!imagesWithMeta.Any())
            return new ProductImageResult();

        var orderedDtos = imagesWithMeta.Select(x => x.LookupDto).ToList();

        var variantDict = imagesWithMeta
            .Where(x => x.OptionValueId.HasValue)
            .GroupBy(x => x.OptionValueId!.Value)
            .ToDictionary(
                g => g.Key,
                g => g.First().LookupDto
            );

        return new ProductImageResult
        {
            MainImage = orderedDtos.FirstOrDefault(),
            CommonImages = orderedDtos,
            VariantImages = variantDict
        };
    }

    public async Task<ImageLookupDto?> GetPrimaryImageAsync(Guid productId, Guid? optionValueId = null, CancellationToken ct = default)
    {
        var spec = new ProductImageSpec()
            .ByProductId(productId)
            .ApplyOrderingBy(optionValueId)
            .TakeOne()
            .WithProjectionOf(new ProductImageProjectionSpec());

        var image = await _productImageRepo.FirstOrDefaultAsync(spec, ct);
        return image?.LookupDto;
    }

    public async Task<bool> BeValidImageRules(UploadFileCommand cmd, CancellationToken ct)
    {
        var spec = new ProductImageSpec()
            .ByProductId(cmd.ProductId)
            .ByOptionValueId(cmd.OptionValueId);

        if (cmd.OptionValueId is null)
        {
            var imgs = await _productImageRepo.ListAsync(spec);
            var main = imgs.Count(x => x.IsMain);
            var subs = imgs.Count(x => !x.IsMain);
            return cmd.IsMain ? main < 1 : subs < 8;
        }
        // If image linked to option value, only one image allowed
        return !await _productImageRepo.AnyAsync(spec);
    }
}

//var selectedVariantId = request.OptionValueId;
//    var orderedList = images
//        .Select(img => new
//        {
//            Dto = new ImageLookupDto { Id = img.Id, Url = img.ImageUrl! },
//            Priority = img.OptionValueId == selectedVariantId ? 100 : // variant
//                       img.IsMain && img.OptionValueId == null ? 50 : // main
//                       img.OptionValueId == null ? 20 : 0, // common
//            PriorityOrder = img.OptionValueId == selectedVariantId ? 0 :
//                           img.IsMain && img.OptionValueId == null ? 1 :
//                           img.OptionValueId == null ? 2 : 3
//        })
//        .OrderByDescending(x => x.Priority)
//        .ThenBy(x => x.PriorityOrder)
//        .ThenBy(x => x.Dto.Id)
//        .Select(x => x.Dto)
//        .ToList();
//var mainImage = images.FirstOrDefault();
//var result = new ProductImageResult
//{
//    MainImage = new ImageLookupDto { Id = mainImage.Id, Url = mainImage.ImageUrl},
//    CommonImages = orderedList,
//    VariantImages = images
//        .Where(x => x.OptionValueId.HasValue)
//        .ToDictionary(
//            x => x.OptionValueId!.Value,
//            x => new ImageLookupDto { Id = x.Id, Url = x.ImageUrl! }
//        )
//};
