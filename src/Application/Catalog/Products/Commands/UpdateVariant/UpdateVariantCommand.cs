using MediatR;

namespace Application.Catalog.Products.Commands.UpdateVariant;

public record UpdateVariantCommand(Guid Id, decimal RegularPrice, int Quantity) : IRequest<Unit>;