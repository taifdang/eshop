

namespace Application.Catalog.Products.Dtos;

public class ProductItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductOptionValueDto> OptionValues { get; set; } = new();
    public List<ProductOptionDto> Options { get; set; } = new();
}
