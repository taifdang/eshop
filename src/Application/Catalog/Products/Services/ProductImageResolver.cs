using Application.Abstractions;
using Application.Common.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Products.Services;

public class ProductImageResolver : IProductImageResolver
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<ProductImageResolver> _logger;

    public ProductImageResolver(
        IApplicationDbContext dbContext,
        ILogger<ProductImageResolver> logger)
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
                  Url = x.Image.BaseUrl + x.Image.FileName
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
              .Where(v => v.ImageId != null)
              .Include(v => v.Image)
              .ToDictionaryAsync(
                x => x.Id,
                x => new ImageLookupDto
                {
                    Id = x.Image!.Id,
                    Url = x.Image.BaseUrl + x.Image.FileName
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
        //    var variantImageDict = await _dbContext.Values
        //        .Where(x => x.Id == optionValueId.Value && x.Image != null)
        //        .ToDictionaryAsync(
        //            x => x.Id,
        //            x => new ImageLookupDto 
        //            { 
        //                Id = x.ImageId.Value, 
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
         .Where(v => v.ImageId != null)
         .Include(v => v.Image)
         .Select(v => new ImageLookupDto
         {
             Id = v.ImageId!.Value,
             Url = v.Image!.BaseUrl + v.Image.FileName
         })
         .ToDictionaryAsync(v => v.Id, v => v, ct);
    }

    public async Task<ImageLookupDto> GetVariantImageAndFallback(Guid productId, List<Guid>? optionValueIds = null, CancellationToken ct = default)
    {
        var result = new ImageLookupDto();

        if(optionValueIds.Count > 0)
        {
            var variantImage = await _dbContext.OptionValues
           .Where(x => optionValueIds.Contains(x.Id) && x.ImageId != null)
           .Include(x => x.Image)
           .Select(y => new ImageLookupDto
           {
               Id = y.ImageId!.Value,
               Url = y.Image!.BaseUrl + y.Image.FileName
           })
           .FirstOrDefaultAsync();

           if (variantImage is not null)
           {
                result = variantImage;
           }
        }
        if(result is null || optionValueIds.Count == 0)
        {
            var fallbackImage = await _dbContext.ProductImages
             .AsNoTracking()
             .Where(x => x.ProductId == productId)
             .Where(v => v.ImageId != null)
             .Include(x => x.Image)
             .OrderByDescending(m => m.IsMain)
             .ThenBy(m => m.SortOrder)
             .ThenBy(x => x.Id)
             .Select(x => new ImageLookupDto
             {
                 Id = x.ImageId,
                 Url = x.Image.BaseUrl + x.Image.FileName
             })
             .FirstOrDefaultAsync(ct);

            result = fallbackImage;
        }

        return result ?? new();
    }
}

//public async Task<bool> BeValidImageRules(UploadFileCommand cmd, CancellationToken ct)
//{
//    var spec = new ProductImageSpec()
//        .ByProductId(cmd.ProductId)
//        .ByOptionValueId(cmd.Id);

//    if (cmd.Id is null)
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


//var selectedVariantId = request.Id;
//    var orderedList = images
//        .Select(img => new
//        {
//            Dto = new ImageLookupDto { Id = img.Id, Url = img.Url! },
//            Priority = img.Id == selectedVariantId ? 100 : // variant
//                       img.IsMain && img.Id == null ? 50 : // main
//                       img.Id == null ? 20 : 0, // common
//            PriorityOrder = img.Id == selectedVariantId ? 0 :
//                           img.IsMain && img.Id == null ? 1 :
//                           img.Id == null ? 2 : 3
//        })
//        .OrderByDescending(x => x.Priority)
//        .ThenBy(x => x.PriorityOrder)
//        .ThenBy(x => x.Dto.Id)
//        .Select(x => x.Dto)
//        .ToList();

