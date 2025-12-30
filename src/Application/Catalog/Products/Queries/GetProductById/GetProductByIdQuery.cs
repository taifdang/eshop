using Application.Catalog.Products.Services;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductItemDto>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductItemDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IImageLookupService _imageLookupService;

    public GetProductByIdQueryHandler(
        IImageLookupService imageLookupService,
        IApplicationDbContext dbContext)
    {
        _imageLookupService = imageLookupService;
        _dbContext = dbContext;
    }

    public async Task<ProductItemDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // *When user click product, we will load all options, option values, images for product to client side
        // *Then user select option values, we will generate variant on client side (based on option values selected)
        // to show Price, sku, quantity, image...

        //var spec = new ProductSpec()
        //    .ById(request.Id)
        //    .WithProjectionOf(new ProductItemProjectionSpec());

        var products = await _dbContext.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == request.Id)
            .Select(m => new ProductItemDto
            {
                Id = m.Id,
                Title = m.Name,
                //MinPrice = m.Variants.Any() ? m.Variants.Min(v => v.Price) : 0m,
                //MaxPrice = m.Variants.Any() ? m.Variants.Max(v => v.Price) : 0m,
                Description = m.Description ?? string.Empty,
                Category = m.Category.Name,
                VariantBrief = m.Variants.Select(v => new { v.Id, v.Price, v.Quantity }).GroupBy(_ => 1).Select(g => new VariantBriefDto
                {
                    Id = g.Count() == 1 ? g.First().Id : null,
                    Quantity = g.Count() == 1 ? g.First().Quantity : 0,
                    MinPrice = g.Min(v => v.Price),
                    MaxPrice = g.Max(v => v.Price),
                }).SingleOrDefault(),
                Options = m.Options.Select(po => new ProductOptionDto
                {
                    Id = po.Id,
                    Title = po.Name,
                    Values = po.Values.Select(ov => new OptionValueDto
                    {
                        Id = ov.Id,
                        Value = ov.Value,
                    }).ToList()
                }).ToList(),

            })
            .FirstOrDefaultAsync();

        if (products == null)
        {
            return null!;
        }

        var imageLookup = await _imageLookupService.GetProductDetailImageAsync(request.Id);

        products.MainImage = imageLookup.MainImage;
        products.Images = imageLookup.CommonImages;

        // variant image = option value image
        if(imageLookup.VariantImages.Count > 0)
        {
            foreach (var option in products.Options)
            {
                foreach (var value in option.Values)
                {
                    if (imageLookup.VariantImages.TryGetValue(value.Id, out var img))
                    {
                        value.Image = img;
                    }
                }
            }
        }

        return products;
    }
}

#region
//var productVm = await _unitOfWork.ProductRepository.GetByIdAsync(
//    filter: x => x.OptionId == request.OptionId,
//    selector: x => new ProductDetailVm
//    {
//        OptionId = x.OptionId,
//        Name = x.Name,
//        MinPrice = x.Variants.Min(x => x.Price),
//        MaxPrice = x.Variants.Max(x => x.Price),
//        Description = x.Description ?? string.Empty,
//        Category = x.Category.Name,
//        ProductType = x.Category.ProductType.Name,
//        Images = x.Images.Select(img => new ImageLookupDto
//        {
//            OptionId = img.OptionId,
//            Url = img.eImage,
//        }).ToList(),
//        Options = x.Options.Select(po => new OptionLookupDto 
//        { 
//            Name = po.Name,
//            Values = po.Values.Select(v => v.Value).ToList()
//        }).ToList(),
//        Values = x.Options.Select(po => new OptionValueDto
//        {
//            Name = po.Name,
//            Values = po.Values.Select(v => v.Value).ToList(),
//            Options = po.Values.Select(ov => new OptionValueImageDto
//            {
//                Name = ov.Value,
//                Label = ov.Label,
//                eImage = ov.Images!.Select(pi => new ImageLookupDto
//                {
//                    OptionId = pi.OptionId,
//                    Url = pi.eImage
//                }).ToList()
//            }).ToList()
//        }).ToList(),
//    });
#endregion