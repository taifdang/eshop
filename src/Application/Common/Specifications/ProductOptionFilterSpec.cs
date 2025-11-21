using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductOptionFilterSpec : Specification<ProductOption>
{
    public ProductOptionFilterSpec(Guid productId, Guid? productOptionId)
    {
        Query
            .Where(x => x.ProductId == productId &&
            (!productOptionId.HasValue || x.Id == productOptionId));
    }
}
