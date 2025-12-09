using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetVariantByOption;

public record GetVariantByOptionQuery(Guid ProductId, List<Guid> OptionValueMap) : IRequest<VariantItemListDto>;

public class GetVariantByOptionQueryHandler : IRequestHandler<GetVariantByOptionQuery, VariantItemListDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetVariantByOptionQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VariantItemListDto> Handle(GetVariantByOptionQuery request, CancellationToken cancellationToken)
    {
        var optionCount = await _dbContext.ProducOptions
            .Where(x => x.ProductId == request.ProductId)
            .CountAsync();

        // boolean
        var exact = request.OptionValueMap.Count == optionCount;

        //var variantSpec = new ProductVariantSpec()
        //    .ByProductId(request.ProductId)
        //    .FilterByOptions(request.OptionValueMap, exact)
        //    .WithProjectionOf(new ProductVariantItemProjectionSpec());

        var query = _dbContext.Variants.AsQueryable();

        // filter by productId
        query = query.Where(x => x.ProductId == request.ProductId);

        // filter by optionValue
        if (request.OptionValueMap.Any())
        {
            // Not enough to option values, need to match all provided options
            query = query.Where(x => request.OptionValueMap
                .All(opt => x.VariantOptions
                    .Any(v => v.OptionValueId == opt)));
            // Corrected to ensure exact match of option values
            // Ex: Color : Red, Size: M  should not match Color: Red, Size: M, Material: Cotton
            if (exact)
                query = query.Where(x => x.VariantOptions.Count == request.OptionValueMap.Count);
        }
        // projection
        var variants = await query
            .Select(x => new VariantItemDto
            {
                Id = x.Id,
                Price = x.Price,
                Quantity = x.Quantity,
                Options = x.VariantOptions.Select(y => new OptionValueLookupDto
                {
                    OptionId = y.OptionValue.OptionId,
                    OptionValueId = y.OptionValueId,
                    Value = y.OptionValue.Value
                })
             .OrderBy(o => o.Value)
             .ToList()
            })
            .ToListAsync(); 

        if (!variants.Any())
        {
            throw new Exception("Not found variant");
        }         

        return new VariantItemListDto
        {
            Variants = variants,
            MinPrice = variants.Min(v => v.Price),
            MaxPrice = variants.Max(v => v.Price),
            TotalStock = variants.Sum(v => v.Quantity)
        };
    }
}
