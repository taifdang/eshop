using MediatR;

namespace Application.Order.Commands.SetPaymentRejectedOrderStatus;

public record SetPaymentRejectedOrderStatusCommand(long OrderNumber) : IRequest<bool>;
