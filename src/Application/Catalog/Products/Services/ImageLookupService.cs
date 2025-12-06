using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Products.Services;

public class ImageLookupService : IImageLookupService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<ImageLookupService> _logger;

    public ImageLookupService(
        IApplicationDbContext dbContext,
        ILogger<ImageLookupService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ProductImageResult> GetProductDetailImageAsync(Guid productId, CancellationToken ct = default)
    {
        var productImage = new ProductImageResult();
        try
        {
            var imageList = await _dbContext.ProductImages
              .AsNoTracking()
              .Where(x => x.ProductId == productId)
              .Where(v => v.ImageId != null && v.Image != null)
              .OrderByDescending(m => m.IsMain)
              .ThenBy(m => m.SortOrder)
              .ThenBy(x => x.Id)
              .Select(x => new ImageLookupDto
              {
                  Id = x.ImageId,
                  Url = x.Image.BaseUrl + "/" + x.Image.FileName
              })
              .ToListAsync(ct);

            if (!imageList.Any())
                return new ProductImageResult();

            productImage.MainImage = imageList.First();
            productImage.CommonImages = imageList;

            var variantImages = await _dbContext.ProducOptions
              .AsNoTracking()
              .Where(o => o.ProductId == productId && o.AllowImage)
              .SelectMany(x => x.Values)
              .Where(v => v.ImageId != null && v.Image != null)
              .ToDictionaryAsync(
                x => x.Id,
                x => new ImageLookupDto
                {
                    Id = x.Image.Id,
                    Url = x.Image.BaseUrl + "/" + x.Image.FileName
                });

            productImage.VariantImages = variantImages;  
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "Error while processing product images");
        }
        #region option value (optional)
        // * Only one option value has image
        //if (optionValueId.HasValue)
        //{
        //    var variantImageDict = await _dbContext.OptionValues
        //        .Where(x => x.OptionValueId == optionValueId.Value && x.Image != null)
        //        .ToDictionaryAsync(
        //            x => x.OptionValueId,
        //            x => new ImageLookupDto 
        //            { 
        //                OptionValueId = x.ImageId.Value, 
        //                Url = x.Image.BaseUrl + "/" + x.Image.FileName 
        //            });

        //    productImage.VariantImages = variantImageDict;
        //}     
        #endregion
        return productImage;
    }

    public async Task<Dictionary<Guid, ImageLookupDto>> GetVariantListImageAsync(Guid productId, Guid? optionValueId = null, CancellationToken ct = default)
    {
        if (!optionValueId.HasValue)
            return new Dictionary<Guid, ImageLookupDto>(0);

        return await _dbContext.ProducOptions
         .AsNoTracking()
         .Where(o => o.ProductId == productId && o.AllowImage)
         .SelectMany(o => o.Values)
         .Where(v => v.Id == optionValueId.Value)
         .Where(v => v.ImageId != null && v.Image != null)
         .Select(v => new ImageLookupDto
         {
             Id = v.ImageId!.Value,
             Url = v.Image.BaseUrl + "/" + v.Image.FileName
         })
         .ToDictionaryAsync(v => v.Id, v => v, ct);
    }
}

//public async Task<bool> BeValidImageRules(UploadFileCommand cmd, CancellationToken ct)
//{
//    var spec = new ProductImageSpec()
//        .ByProductId(cmd.ProductId)
//        .ByOptionValueId(cmd.OptionValueId);

//    if (cmd.OptionValueId is null)
//    {
//        var imgs = await _productImageRepo.ListAsync(spec);
//        var main = imgs.Count(x => x.IsMain);
//        var subs = imgs.Count(x => !x.IsMain);
//        return cmd.IsMain ? main < 1 : subs < 8;
//    }
//    // If image linked to option value, only one image allowed
//    return !await _productImageRepo.AnyAsync(spec);
//}

//var spec = new ProductImageSpec()
//          .ByProductId(productId)
//          .ApplyOrderingBy(optionValueId)
//          .TakeOne()
//          .WithProjectionOf(new ProductImageProjectionSpec());

//var spec = new ProductImageSpec()
//    .ByProductId(productId)
//    .ApplyOrderingBy(optionValueId)
//    .TakeOne()
//    .WithProjectionOf(new ProductImageProjectionSpec());   


//var selectedVariantId = request.OptionValueId;
//    var orderedList = images
//        .Select(img => new
//        {
//            Dto = new ImageLookupDto { OptionValueId = img.OptionValueId, Url = img.Url! },
//            Priority = img.OptionValueId == selectedVariantId ? 100 : // variant
//                       img.IsMain && img.OptionValueId == null ? 50 : // main
//                       img.OptionValueId == null ? 20 : 0, // common
//            PriorityOrder = img.OptionValueId == selectedVariantId ? 0 :
//                           img.IsMain && img.OptionValueId == null ? 1 :
//                           img.OptionValueId == null ? 2 : 3
//        })
//        .OrderByDescending(x => x.Priority)
//        .ThenBy(x => x.PriorityOrder)
//        .ThenBy(x => x.Dto.OptionValueId)
//        .Select(x => x.Dto)
//        .ToList();

