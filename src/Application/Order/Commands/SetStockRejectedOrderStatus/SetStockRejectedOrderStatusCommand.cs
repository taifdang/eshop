using MediatR;

namespace Application.Order.Commands.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
