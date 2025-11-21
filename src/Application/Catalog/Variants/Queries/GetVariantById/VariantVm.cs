using Application.Common.Models;

namespace Application.Catalog.Variants.Queries.GetVariantById;

public class VariantVm
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal Percent { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public ImageLookupDto? Image { get; set; }
    public List<OptionLookupDto> Options { get; set; }
}
