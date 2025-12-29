using Application.Common.Models;

namespace Application.Catalog.Products.Queries.GetProductById;

public class ProductItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    //public decimal MinPrice { get; init; }
    //public decimal MaxPrice { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public VariantBriefDto? VariantBrief { get; set; }
    public ImageLookupDto? MainImage { get; set; }
    public List<ImageLookupDto>? Images { get; set; }
    public List<ProductOptionDto> Options { get; set; }
}
