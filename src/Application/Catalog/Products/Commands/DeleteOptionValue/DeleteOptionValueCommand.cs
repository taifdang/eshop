using MediatR;

namespace Application.Catalog.Products.Commands.DeleteOptionValue;

public record DeleteOptionValueCommand(Guid ProductId, Guid OptionId, Guid OptionValueId) : IRequest<Unit>;
