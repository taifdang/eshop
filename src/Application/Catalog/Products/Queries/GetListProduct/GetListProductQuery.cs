using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Application.Common.Models;
using Ardalis.Specification;
using MediatR;

namespace Application.Catalog.Products.Queries.GetListProduct;

public record GetListProductQuery(int PageIndex, int PageSize) : IRequest<PageList<ProductListDto>>;

public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, PageList<ProductListDto>>
{
    private readonly IRepository<Domain.Entities.Product> _productRepository;
    public GetListProductQueryHandler(IRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<PageList<ProductListDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductSpec()
            .WithIsPublished()
            .ApplyPaging(request.PageIndex * request.PageSize, request.PageSize)
            .WithProjectionOf(new ProductListProjectionSpec());

        var productList = await _productRepository.ListAsync(spec, cancellationToken);

        return new PageList<ProductListDto>(
           productList!,
           productList.Count,
           request.PageIndex,
           request.PageSize);   
    }
}
