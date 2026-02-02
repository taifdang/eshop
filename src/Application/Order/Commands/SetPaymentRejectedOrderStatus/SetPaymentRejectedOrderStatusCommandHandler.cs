using Domain.Repositories;
using MediatR;

namespace Application.Order.Commands.SetPaymentRejectedOrderStatus;

public class SetPaymentRejectedOrderStatusCommandHandler : IRequestHandler<SetPaymentRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetPaymentRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetPaymentRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(command.OrderNumber);

        if (order == null)
        {
            return false;
        }

        order.SetRejectedStatusWhenPaymentRejected();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
