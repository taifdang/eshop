using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Products.Commands.CreateProductImage;

public record CreateProductImageCommand(Guid ProductId, bool IsMain = false) : IRequest<Unit>
{
    public IFormFile MediaFile { get; init; }
}
