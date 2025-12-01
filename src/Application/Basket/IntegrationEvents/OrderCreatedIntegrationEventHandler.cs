using Application.Basket.Specifications;
using Application.Common.Interfaces.Eventbus;
using Application.Common.Interfaces.Persistence;
using Shared.Constracts.Eventbus.Messages;

namespace Application.Basket.IntegrationEvents;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreated>
{
    private readonly IRepository<Domain.Entities.Basket> _basketRepository;

    public OrderCreatedIntegrationEventHandler(IRepository<Domain.Entities.Basket> basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task HandleAsync(OrderCreated e, CancellationToken cancellationToken = default)
    {
        // Note: clear basket after order created

        var spec = new BasketSpec()
         .ByCustomerId(e.CustomerId)
         .WithItems();

        var basket = await _basketRepository.FirstOrDefaultAsync(spec);

        if (basket != null)
        {
            basket.ClearItems();
            await _basketRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
