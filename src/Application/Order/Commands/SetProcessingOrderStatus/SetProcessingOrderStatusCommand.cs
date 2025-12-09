using MediatR;

namespace Application.Order.Commands.SetProcessingOrderStatus;

public record SetProcessingOrderStatusCommand(Guid OrderId) : IRequest<bool>;