using Application.Catalog.Products.Services;
using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Queries.GetVariantById;

public record GetVariantByIdQuery(Guid Id) : IRequest<VariantDto>;

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IRepository<ProductVariant> _productVariantRepo;
    private readonly IMapper _mapper;
    private readonly IProductImageService _productImageService;

    public GetVariantByIdQueryHandler(
        IRepository<ProductVariant> productVariantRepository,
        IMapper mapper,
        IProductImageService productImageService)
    {
        _productVariantRepo = productVariantRepository;
        _mapper = mapper;
        _productImageService = productImageService;
    }
    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductVariantSpec()
            .ByVariantId(request.Id)
            .WithProjectionOf(new ProductVariantProjectionSpec());

        var variant = await _productVariantRepo.FirstOrDefaultAsync(spec, cancellationToken);

        Guard.Against.NotFound(nameof(variant), variant);
       
        var optionValueIdSeleted = variant.Options
            .Where(x => x.IsImage)
            .Select(x => x.OptionValueId).FirstOrDefault();

        var image = await _productImageService.GetPrimaryImageAsync(variant.ProductId, optionValueIdSeleted);

        variant.Image = image ?? null;  

        return _mapper.Map<VariantDto>(variant);
    }
}
// linq order with priority 0 -> 1 -> 2
//var image = await _context.ProductImages
//    .Where(x => x.ProductId == variant.ProductId)
//    .OrderByDescending(x => x.OptionValueId == optionValueSeleted)
//    .ThenByDescending(x => x.IsMain && x.OptionValueId == null)
//    .ThenByDescending(x => x.OptionValueId == null)
//    .ThenBy(x => x.Id)
//    .Select(x => new ImageLookupDto { Id = x.Id, Url = x.ImageUrl })
//    .FirstOrDefaultAsync();