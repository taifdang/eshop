using Application.Common.Models;

namespace Application.Catalog.Products.Queries.GetAvailableProducts;

public class AvailableProductsDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; }
    public ImageLookupDto Image { get; init; }
}
