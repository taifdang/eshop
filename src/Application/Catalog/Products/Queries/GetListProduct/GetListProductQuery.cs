using Application.Common.Dtos;
using Ardalis.Specification;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetListProduct;

public record GetListProductQuery(int PageIndex, int PageSize) : IRequest<PageList<ProductListDto>>;

public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, PageList<ProductListDto>>
{
    private readonly IReadRepository<Product, Guid> _readRepository;

    public GetListProductQueryHandler(IReadRepository<Product, Guid> readRepository)
    {
        _readRepository = readRepository;
    }
    public async Task<PageList<ProductListDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        // logic : for admin, query or edit
        var query = _readRepository.GetQueryableSet();

        // paging
        var page = request.PageIndex <= 0 ? 1 :request.PageIndex;
        var take = request.PageSize;
        var skip = (page - 1) * request.PageSize;
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

        // return paged list
        return new PageList<ProductListDto>(
           productList!,
           productList.Count,
           request.PageIndex,
           request.PageSize);   
    }
}
