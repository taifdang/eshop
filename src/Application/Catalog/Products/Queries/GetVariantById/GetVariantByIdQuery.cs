using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetVariantById;

public record GetVariantByIdQuery(Guid Id) : IRequest<VariantDto>;

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IReadRepository<Variant, Guid> _readRepository;
    private readonly IProductImageResolver _imageResolver;

    public GetVariantByIdQueryHandler(
        IReadRepository<Variant, Guid> readRepository, 
        IProductImageResolver imageResolver)
    {
        _readRepository = readRepository;
        _imageResolver = imageResolver;
    }
    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var variant = await _readRepository
            .GetQueryableSet()
            .Where(x => x.Id == request.Id)
            .Select(x => new VariantDto
            {
                Id = x.Id,
                Title = x.Title,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                Sku = x.Sku,
                Options = x.VariantOptions.Select(y => new VariantOptionDto
                {
                    Id = y.OptionValueId,
                    Value = y.OptionValue.Value
                }).ToList()
            })
            .FirstOrDefaultAsync();

        Guard.Against.NotFound(nameof(variant), variant);

        // get option value
        var optionValueIds = variant.Options.Select(x => x.Id).ToList();

        // get image and fallback
        var variantImage = await _imageResolver.GetVariantImageAndFallback(variant.ProductId, optionValueIds);

        if(variantImage != null)
        {
            variant.Image = variantImage;
        }

        return variant;
    }
}
// linq order with priority 0 -> 1 -> 2
//var image = await _context.Images
//    .Where(x => x.ProductId == variant.ProductId)
//    .OrderByDescending(x => x.Id == optionValueSeleted)
//    .ThenByDescending(x => x.IsMain && x.Id == null)
//    .ThenByDescending(x => x.Id == null)
//    .ThenBy(x => x.Id)
//    .Select(x => new ImageLookupDto { Id = x.Id, Url = x.Url })
//    .FirstOrDefaultAsync();