using Application.Common.Models;

namespace Application.Catalog.Products.Queries.GetProductById;

public class OptionValueDto
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public ImageLookupDto? Image { get; set; }
}
