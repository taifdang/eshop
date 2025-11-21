using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Specifications;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Variants.Queries.GetVariantById;

public record GetVariantByIdQuery(Guid Id) : IRequest<VariantDto>;

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetVariantByIdQueryHandler(
        IRepository<ProductVariant> productVariantRepository,
        IApplicationDbContext context,
        IMapper mapper)
    {
        _productVariantRepository = productVariantRepository;
        _context = context;
        _mapper = mapper;
    }
    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductVariantByIdSpec(request.Id);
        var variant = await _productVariantRepository.FirstOrDefaultAsync(spec, cancellationToken);
       
        if (variant is null)
            Guard.Against.NotFound(nameof(variant), variant);

        Guid? optionHaveImage = variant.Options
            .Where(x => x.IsImage)
            .Select(x => x.OptionValueId).FirstOrDefault();

        // linq order with priority 0 -> 1 -> 2
        var image = await _context.ProductImages
            .Where(x => x.ProductId == variant.ProductId)
            .OrderByDescending(x => x.OptionValueId == optionHaveImage)
            .ThenByDescending(x => x.IsMain)
            .ThenBy(x => x.Id)
            .Select(x => new ImageLookupDto { Id = x.Id, Url = x.ImageUrl })
            .FirstOrDefaultAsync();

        variant.Image = image;  

        return _mapper.Map<VariantDto>(variant);
    }
}
