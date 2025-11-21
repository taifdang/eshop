namespace Application.Catalog.Products.Queries.GetProductById;

public class ProductOptionDto
{
    public string Title { get; init; }
    public List<ProductOptionValueDto> Values { get; init; }
}
