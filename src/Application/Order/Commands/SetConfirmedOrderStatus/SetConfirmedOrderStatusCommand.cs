using Application.Common.Interfaces.Persistence;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Order.Commands.SetConfirmedOrderStatus;

public record SetConfirmedOrderStatusCommand(Guid OrderId) : IRequest<bool>;

public class SetConfirmedOrderStatusCommandHandler : IRequestHandler<SetConfirmedOrderStatusCommand, bool>
{
    private readonly IRepository<Domain.Entities.Order> _orderRepository;

    public SetConfirmedOrderStatusCommandHandler(IRepository<Domain.Entities.Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(SetConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId);

        if (order == null)
        {
            return false;
        }

        order.SetConfirmedStatus(command.OrderId);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}