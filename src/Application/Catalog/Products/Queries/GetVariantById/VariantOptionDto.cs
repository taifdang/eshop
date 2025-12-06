using Application.Common.Models;

namespace Application.Catalog.Products.Queries.GetVariantById;

public class VariantOptionDto
{
    public Guid OptionValueId { get; set; }
    public string Value { get; set; }
    public ImageLookupDto? Image {  get; set; }
}
