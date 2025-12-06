using MediatR;

namespace Application.Catalog.Products.Commands.DeleteOption;

public record DeleteOptionCommand(Guid OptionId, Guid ProductId) : IRequest<Unit>;


