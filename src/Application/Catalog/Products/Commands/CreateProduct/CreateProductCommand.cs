using MediatR;

namespace Application.Catalog.Products.Commands.CreateProduct;

public record CreateProductCommand(Guid CategoryId, string Name,string UrlSlug, string Description) : IRequest<Guid>;