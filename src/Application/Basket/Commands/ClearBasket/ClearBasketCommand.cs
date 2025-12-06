using MediatR;

namespace Application.Basket.Commands.ClearBasket;

public record ClearBasketCommand(Guid CustomerId) : IRequest<Unit>;