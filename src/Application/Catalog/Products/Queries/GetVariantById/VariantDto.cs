using Application.Common.Dtos;

namespace Application.Catalog.Products.Queries.GetVariantById;

public class VariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public ImageLookupDto? Image { get; set; }
    public List<VariantOptionDto> Options { get; set; }
}
