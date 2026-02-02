using MediatR;

namespace Application.Catalog.Products.Commands.UpdateVariant;

public record UpdateVariantCommand(Guid ProductId, Guid Id, decimal RegularPrice, int Quantity) : IRequest<Unit>;
