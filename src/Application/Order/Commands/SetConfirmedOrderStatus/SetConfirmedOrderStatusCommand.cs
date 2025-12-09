using MediatR;

namespace Application.Order.Commands.SetConfirmedOrderStatus;

public record SetConfirmedOrderStatusCommand(
    long OrderNumber,
    string? CardBrand,
    string? TransactionId) : IRequest<bool>;
