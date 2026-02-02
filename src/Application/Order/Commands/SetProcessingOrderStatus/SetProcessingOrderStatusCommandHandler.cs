using Domain.Repositories;
using MediatR;

namespace Application.Order.Commands.SetProcessingOrderStatus;

public class SetProcessingOrderStatusCommandHandler : IRequestHandler<SetProcessingOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetProcessingOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetProcessingOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetProcessingStatus();

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
