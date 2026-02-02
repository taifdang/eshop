using Domain.Repositories;
using MediatR;

namespace Application.Order.Commands.SetStockRejectedOrderStatus;

public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetRejectedStatusWhenStockRejected();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
