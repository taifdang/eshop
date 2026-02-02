using Application.Common.Dtos;

namespace Application.Catalog.Products.Queries.GetListProduct;

public class ProductListDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Category { get; init; }
    public ImageLookupDto Image { get; init; }
}
