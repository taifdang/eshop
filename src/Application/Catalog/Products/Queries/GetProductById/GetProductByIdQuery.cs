using Application.Catalog.Products.Services;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductItemDto>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductItemDto>
{
    private readonly IReadRepository<Product, Guid> _readRepository;
    private readonly IProductImageResolver _imageResolver;

    public GetProductByIdQueryHandler(
        IProductImageResolver imageResolver,
        IReadRepository<Product, Guid> readRepository)
    {
        _imageResolver = imageResolver;
        _readRepository = readRepository;
    }

    public async Task<ProductItemDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // Scenario:
        // *When user click product, we will load all options, option values, images for product to client side
        // *Then user select option values, we will load data (variant, quantity, price, image ...) on client side (based on option values selected)

        var products = await _readRepository
            .GetQueryableSet()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Id == request.Id)
            .Select(m => new ProductItemDto
            {
                Id = m.Id,
                Title = m.Name,
                Description = m.Description ?? string.Empty,
                Category = m.Category.Name,
                VariantSummary = m.Variants.Select(v => new { v.Id, v.Price, v.Quantity }).GroupBy(_ => 1).Select(g => new VariantSummaryDto
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

        var imageLookup = await _imageResolver.GetProductDetailImageAsync(request.Id);

        products.MainImage = imageLookup.MainImage;
        products.Images = imageLookup.CommonImages;

        // mapping image with option values
        if (imageLookup.VariantImages.Count > 0)
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