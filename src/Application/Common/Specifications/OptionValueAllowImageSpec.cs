using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class OptionValueAllowImageSpec : Specification<OptionValue>
{
    public OptionValueAllowImageSpec(Guid optionValueId, Guid productId)
    {
        Query   
            .Where(x => 
                x.ProductOption.ProductId == productId &&
                x.Id == optionValueId && 
                x.ProductOption.AllowImage)
            .Include(x => x.ProductOption);
    }
}
