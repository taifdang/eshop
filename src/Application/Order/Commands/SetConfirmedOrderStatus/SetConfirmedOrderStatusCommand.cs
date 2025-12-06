using Application.Common.Interfaces;
using MediatR;

namespace Application.Order.Commands.SetConfirmedOrderStatus;

public record SetConfirmedOrderStatusCommand(Guid OrderId) : IRequest<bool>;

public class SetConfirmedOrderStatusCommandHandler : IRequestHandler<SetConfirmedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetConfirmedOrderStatusCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(command.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetConfirmedStatus(command.OrderId);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}