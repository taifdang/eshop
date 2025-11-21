using Application.Common.Models;

namespace Application.Catalog.Variants.Queries.GetVariantByOption;

public class VariantItemDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public decimal Percent { get; set; }
    public int Quantity { get; set; }
    public List<OptionValueLookupDto> Options { get; set; }
}
