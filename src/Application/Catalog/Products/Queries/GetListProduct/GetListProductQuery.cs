using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Specifications;
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
        var specification = new ProductListPaginationSpec(request.PageIndex * request.PageSize, take: request.PageSize);
        var productList = await _productRepository.ListAsync(specification, cancellationToken);

        return new PageList<ProductListDto>(
           productList!,
           productList.Count,
           request.PageIndex,
           request.PageSize);
        #region
        //var listProductVm = await _unitOfWork.ProductRepository.ToPagination(
        //        PageIndex: request.PageIndex,
        //        PageSize: request.PageSize,
        //        orderBy: x => x.ProductOptionId,
        //        ascending: true,
        //        selector: x => new ProductListDto
        //        {
        //            ProductOptionId = x.ProductOptionId,
        //            Title = x.Title,
        //            //MinPrice = x.MinPrice,
        //            //MaxPrice = x.MaxPrice,
        //            Price = x.ProductVariants.Min(pv => pv.Price), // Min Price of all variants
        //            Description = x.Description ?? string.Empty,
        //            Category = x.Category.Title,
        //            ProductType = x.Category.ProductType.Title,
        //            Image = x.ProductImages
        //            .Where(c => c.IsMain && c.ProductOptionId == null) // Main image not linked to option value
        //            .Select(pi => new ImageLookupDto
        //            {
        //                ProductOptionId = pi.ProductOptionId,
        //                Url = pi.Image
        //            })
        //            .FirstOrDefault() ?? new(),
        //        });
        #endregion
        #region List product with options and option values
        //var listProductVm = await _orderRepository.ProductRepository.ToPagination(
        //        PageIndex: request.PageIndex,
        //        PageSize: request.PageSize,
        //        orderBy: x => x.ProductOptionId,
        //        ascending: true,
        //        selector: x => new ProductVm
        //        {
        //            ProductOptionId = x.ProductOptionId,
        //            Title = x.Title,
        //            MinPrice = x.MinPrice,
        //            MaxPrice = x.MaxPrice,
        //            Description = x.Description ?? string.Empty,
        //            Category = x.Category.Title,
        //            ProductType = x.Category.ProductType.Title,
        //            Images = x.ProductImages.Select(img => new ImageLookupDto
        //            {
        //                ProductOptionId = img.ProductOptionId,
        //                Image = img.Image
        //            }).ToList(),
        //            Options = x.ProductOptions.Select(po => new OptionLookupDto
        //            {
        //                Title = po.OptionName,
        //                Values = po.OptionValues.Select(v => v.Value).ToList()
        //            }).ToList(),
        //            OptionValues = x.ProductOptions.Select(po => new OptionValueDto
        //            {
        //                Title = po.OptionName,
        //                Values = po.OptionValues.Select(v => v.Value).ToList(),
        //                Options = po.OptionValues.Select(ov => new OptionValueImageDto
        //                {
        //                    Title = ov.Value,
        //                    Label = ov.Label,
        //                    Image = ov.ProductImages!.Select(pi => new ImageLookupDto
        //                    {
        //                        ProductOptionId = pi.ProductOptionId,
        //                        Image = pi.Image
        //                    }).ToList()
        //                }).ToList()
        //            }).ToList()
        //        });
        #endregion


    }
}
