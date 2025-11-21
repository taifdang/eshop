namespace Application.Catalog.Variants.Queries.GetVariantByOption;

public class AvailableVariant
{
    public IReadOnlyList<VariantItemDto> Variants { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int TotalStock { get; set; }
}
