using Application.Common.Models;

namespace Application.Catalog.Products.Services;

public class ProductImageResult
{
    public ImageLookupDto MainImage { get; set; }
    public List<ImageLookupDto> CommonImages { get; set; }
    public Dictionary<Guid, ImageLookupDto> VariantImages { get; set; }
}
