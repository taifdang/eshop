namespace Application.Catalog.Products.Queries.GetVariantByOption;

public class VariantItemListDto
{
    public IReadOnlyList<VariantItemDto> Variants { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int TotalStock { get; set; }
}
