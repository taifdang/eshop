using Application.Common.Interfaces;
using MediatR;

namespace Application.Order.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetCancelledStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
