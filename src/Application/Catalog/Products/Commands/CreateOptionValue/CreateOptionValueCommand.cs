using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public record CreateOptionValueCommand(Guid OptionId, string Value) : IRequest<Unit>
{
    public IFormFile? MediaFile { get; init; }
}