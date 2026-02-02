using Domain.Repositories;
using MediatR;

namespace Application.Order.Commands.SetConfirmedOrderStatus;

public class SetConfirmedOrderStatusCommandHandler : IRequestHandler<SetConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumber(command.OrderNumber);

        if (order == null)
        {
            return false;
        }

        order.CardBrand = command.CardBrand;
        order.TransactionId = command.TransactionId;

        order.SetConfirmedStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
