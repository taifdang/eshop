using MediatR;

namespace Application.Order.Commands.SetCompletedOrderStatus;

public record SetCompletedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
