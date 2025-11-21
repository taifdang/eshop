using Ardalis.Specification;

namespace Application.Common.Specifications;

public class BasketWithItemsBySpec : Specification<Domain.Entities.Basket>
{
    public BasketWithItemsBySpec(Guid customerId)
    {
        Query
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.Items);
    }
}
