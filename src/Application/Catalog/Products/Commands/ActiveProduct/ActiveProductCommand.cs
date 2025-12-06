using MediatR;

namespace Application.Catalog.Products.Commands.ActiveProduct;

public record ActiveProductCommand(Guid ProductId, bool IsActive) : IRequest<Unit>;
