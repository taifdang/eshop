namespace Application.Catalog.Products.Dtos;

public class ProductListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; } // Min Price of all variants
    public string Description { get; set; }
    public string Category { get; set; }
    public ProductImageDto Image { get; set; } // Main image URL
}
