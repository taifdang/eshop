using MediatR;

namespace Application.Catalog.Products.Commands.DeleteOptionValue;

public record DeleteOptionValueCommand(Guid OptionValueId, Guid OptionId) : IRequest<Unit>;
