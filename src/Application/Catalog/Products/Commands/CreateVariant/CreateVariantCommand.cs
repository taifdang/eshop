using MediatR;

namespace Application.Catalog.Products.Commands.CreateVariant;

public record CreateVariantCommand(Guid ProductId, decimal RegularPrice, int Quantity) : IRequest<Guid>;