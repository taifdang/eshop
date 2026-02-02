using Application.Common.Dtos;

namespace Application.Catalog.Products.Services;

public interface IProductImageResolver
{
    Task<ProductImageResult> GetProductDetailImageAsync(
        Guid productId,
        CancellationToken ct = default);

    Task<Dictionary<Guid, ImageLookupDto>> GetVariantListImageAsync(
       Guid productId,
       Guid? optionValueId = null,
       CancellationToken ct = default);

    Task<ImageLookupDto> GetVariantImageAndFallback(
       Guid productId,
       List<Guid>? optionValueId = null,
       CancellationToken ct = default);

    //Task<bool> BeValidImageRules(
    //    UploadFileCommand cmd,
    //    CancellationToken ct);
}

public class ProductImageResult
{
    public ImageLookupDto MainImage { get; set; }
    public List<ImageLookupDto> CommonImages { get; set; }
    public Dictionary<Guid, ImageLookupDto> VariantImages { get; set; }
}
