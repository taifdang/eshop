using Domain.Repositories;
using MediatR;

namespace Application.Basket.Commands.ClearBasket;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Guid>
{
    private readonly IBasketRepository _repository;

    public ClearBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _repository.GetByCustomerIdWithItemsAsync(request.CustomerId, cancellationToken);

        if (basket is null)
        {
            throw new Exception("Not found basket");
        }

        basket.ClearItems();
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}