using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public record CreateOptionValueCommand(Guid ProductId, Guid OptionId, string Value, IFormFile? MediaFile = null) : IRequest<Unit>;
