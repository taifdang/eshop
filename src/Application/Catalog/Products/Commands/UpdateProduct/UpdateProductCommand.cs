using MediatR;

namespace Application.Catalog.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Id, Guid CategoryId, string Title, string Description) : IRequest<Unit>;