using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Events;
using MediatR;

namespace Application.Basket.EventHandlers;

public class ClearBasketOnCheckoutEventHandler : INotificationHandler<BasketShouldBeClearedEvent>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepo;
    public ClearBasketOnCheckoutEventHandler(IRepository<Domain.Entities.Basket> basketRepo)
    {
        _basketRepo = basketRepo;
    }
    public async Task Handle(BasketShouldBeClearedEvent notification, CancellationToken cancellationToken)
    {
        var spec = new BasketWithItemsBySpec(notification.CustomerId);
        var basket = await _basketRepo.FirstOrDefaultAsync(spec);

        if (basket != null)
        {
            await _basketRepo.DeleteAsync(basket, cancellationToken);
        }
    }
}
