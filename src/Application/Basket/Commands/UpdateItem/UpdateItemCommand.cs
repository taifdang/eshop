using MediatR;

namespace Application.Basket.Commands.UpdateItem;

public record UpdateItemCommand(Guid Id, int Quantity) : IRequest<Guid>;