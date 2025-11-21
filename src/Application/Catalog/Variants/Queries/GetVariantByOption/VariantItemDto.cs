using Application.Catalog.Variants.Queries.GetVariantById;

namespace Application.Catalog.Variants.Queries.GetVariantByOption;

public class VariantItemDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal Percent { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public List<VariantOptionValueDto> Options { get; set; }
}
