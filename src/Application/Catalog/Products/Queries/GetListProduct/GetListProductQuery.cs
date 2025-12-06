using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.Specification;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetListProduct;

public record GetListProductQuery(int PageIndex, int PageSize) : IRequest<PageList<ProductListDto>>;

public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, PageList<ProductListDto>>
{
    private readonly IApplicationDbContext _dbContext;
    public GetListProductQueryHandler( IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PageList<ProductListDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        //var spec = new ProductSpec()
        //    .WithIsPublished()
        //    .ApplyPaging(request.PageIndex * request.PageSize, request.PageSize)
        //    .WithProjectionOf(new ProductListProjectionSpec());
        
        var query = _dbContext.Products.AsQueryable();
        // isActive filter
        query = query.Where(p => p.IsActive);
        // paging
        var take = request.PageSize;
        var skip = request.PageIndex * request.PageSize;
        if (take == 0)
        {
            take = int.MaxValue;
        }
        query = query.OrderBy(x => x.Id).Skip(skip).Take(take);
        // projection    
        var productList = await query.Select(x => new ProductListDto
        {
            Id = x.Id,
            Title = x.Name,
            Price = x.Variants.Min(v => v.Price),
            Category = x.Category.Name,
            Image = x.Images
                .OrderByDescending(x => x.IsMain)
                .ThenBy(x => x.SortOrder)
                .Select(x => new ImageLookupDto
                {
                    Id = x.Id,
                    Url = x.Image.BaseUrl + "/" + x.Image.FileName
                })
                .FirstOrDefault() ?? new()
        }).ToListAsync(cancellationToken);

        return new PageList<ProductListDto>(
           productList!,
           productList.Count,
           request.PageIndex,
           request.PageSize);   
    }
}
