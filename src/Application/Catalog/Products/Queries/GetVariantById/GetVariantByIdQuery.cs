using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetVariantById;

public record GetVariantByIdQuery(Guid Id) : IRequest<VariantDto>;

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetVariantByIdQueryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        //var spec = new ProductVariantSpec()
        //    .ByVariantId(request.OptionValueId)
        //    .WithProjectionOf(new ProductVariantProjectionSpec());

        var variant = await _dbContext.Variants
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
                    OptionValueId = y.OptionValueId,
                    Value = y.OptionValue.Value,
                    Image = new ImageLookupDto
                    {
                        Id = y.OptionValue.Image.Id,
                        Url = y.OptionValue.Image.BaseUrl + "/" + y.OptionValue.Image.FileName
                    }           
                }).ToList()
            })
            .FirstOrDefaultAsync();

        Guard.Against.NotFound(nameof(variant), variant);

        return variant;
    }
}
// linq order with priority 0 -> 1 -> 2
//var image = await _context.Images
//    .Where(x => x.ProductId == variant.ProductId)
//    .OrderByDescending(x => x.OptionValueId == optionValueSeleted)
//    .ThenByDescending(x => x.IsMain && x.OptionValueId == null)
//    .ThenByDescending(x => x.OptionValueId == null)
//    .ThenBy(x => x.OptionValueId)
//    .Select(x => new ImageLookupDto { OptionValueId = x.OptionValueId, Url = x.Url })
//    .FirstOrDefaultAsync();