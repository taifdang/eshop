using MediatR;

namespace Application.Order.Commands.CreateOrder;

public record CreateOrderCommand(Guid CustomerId, string Street, string City, string ZipCode) : IRequest<Guid>;
