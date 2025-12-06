using Application.Common.Models;

namespace Application.Catalog.Products.Services;

public interface IImageLookupService
{
    Task<ProductImageResult> GetProductDetailImageAsync(
        Guid productId,
        CancellationToken ct = default);

    Task<Dictionary<Guid, ImageLookupDto>> GetVariantListImageAsync(
       Guid productId,
       Guid? optionValueId = null,
       CancellationToken ct = default);

    //Task<bool> BeValidImageRules(
    //    UploadFileCommand cmd,
    //    CancellationToken ct);
}
