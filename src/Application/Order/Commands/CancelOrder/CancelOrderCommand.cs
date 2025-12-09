using MediatR;

namespace Application.Order.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;
