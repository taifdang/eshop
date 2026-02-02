using Domain.Entities;

namespace Application.Catalog.Products.Services;

public interface IProductVariantService
{
    Task GenerateVariantsAsync(
        Product product, 
        Dictionary<Guid, List<Guid>> optionValues, 
        CancellationToken cancellationToken = default);
}
