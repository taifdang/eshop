using Application.Common.Dtos;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetAvailableProducts;

public record GetAvailableProductsQuery(int PageIndex, int PageSize) : IRequest<PageList<AvailableProductsDto>>;

public class GetAvalableProductQueryhandler : IRequestHandler<GetAvailableProductsQuery, PageList<AvailableProductsDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAvalableProductQueryhandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PageList<AvailableProductsDto>> Handle(GetAvailableProductsQuery request, CancellationToken cancellationToken)
    {
        // enable queryable
        var query = _productRepository.GetQueryableSet();

        // filter
        query = query.Where(p => p.IsActive);

        // paging
        var page = request.PageIndex <= 0 ? 1 : request.PageIndex;
        var take = request.PageSize;
        var skip = (page - 1) * request.PageSize;
        if (take == 0)
        {
            take = int.MaxValue;
        }
        query = query.OrderBy(x => x.Id).Skip(skip).Take(take);

        // projection    
        var productList = await query.Select(x => new AvailableProductsDto
        {
            Id = x.Id,
            Title = x.Name,
            Price = x.Variants.Select(v => (decimal?)v.Price).Min() ?? 0,
            Category = x.Category.Name,
            Image = x.Images
                .OrderByDescending(x => x.IsMain)
                .ThenBy(x => x.SortOrder)
                .Select(x => new ImageLookupDto
                {
                    Id = x.Id,
                    Url = x.Image.BaseUrl + x.Image.FileName
                })
                .FirstOrDefault() ?? new()
        }).ToListAsync();

        return new PageList<AvailableProductsDto>(
           productList!,
           productList.Count,
           request.PageIndex,
           request.PageSize);
    }
}
