using Application.Common.Interfaces;
using MediatR;

namespace Application.Order.Commands.SetCompletedOrderStatus;

public class SetCompletedOrderStatusCommandHandler : IRequestHandler<SetCompletedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetCompletedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetCompletedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetCompletedStatus();  

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
