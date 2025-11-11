using Application.Common.Interfaces;
using Application.Common.Specifications;
using MediatR;

namespace Application.Basket.Commands.ClearBasket;

public record ClearBasketCommand(int customerId) : IRequest<Unit>;

public class ClearCartCommandHandler : IRequestHandler<ClearBasketCommand, Unit>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepository;
    public ClearCartCommandHandler(IRepository<Domain.Entities.Basket> basketRepository)
    {
        _basketRepository = basketRepository;
    }
    public async Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.FirstOrDefaultAsync(new BasketCustomerWithItemSpec(request.customerId));
        if (basket != null)
        {          
            await _basketRepository.DeleteRangeAsync((IEnumerable<Domain.Entities.Basket>)basket.Items, cancellationToken);
        }
        return Unit.Value;
    }
}

