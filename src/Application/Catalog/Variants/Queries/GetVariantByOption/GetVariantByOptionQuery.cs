using Application.Catalog.Variants.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Queries.GetVariantByOption;

public record GetVariantByOptionQuery : IRequest<VariantListDto>
{
    public int ProductId { get; init; }
    public Dictionary<int, int> OptionFilter { get; init; }
}

public class GetVariantByOptionQueryHandler : IRequestHandler<GetVariantByOptionQuery, VariantListDto>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    private readonly IRepository<ProductOption> _productOptionRepository;

    public GetVariantByOptionQueryHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }
    public async Task<VariantListDto> Handle(GetVariantByOptionQuery request, CancellationToken cancellationToken)
    {
       
        var totalOptions = await _productOptionRepository.CountAsync(
            new OptionProductFilterSpec(request.ProductId, null),
            cancellationToken);

        var isExactMatch = request.OptionFilter.Count == totalOptions;

        var variants = await _productVariantRepository.ListAsync(
            new VariantOptionFilterSpec(request.ProductId, request.OptionFilter, isExactMatch), 
            cancellationToken);

        if (!variants.Any())
            throw new EntityNotFoundException(nameof(VariantDto), "No variant found matching the specified options.");
        
        var exactVariant = isExactMatch && variants.Count == 1 ? variants.First() : null;

        decimal minPrice = variants.Min(v => v.RegularPrice);
        decimal maxPrice = variants.Max(v => v.RegularPrice);

        if(exactVariant != null)
            minPrice = maxPrice = exactVariant.RegularPrice;

        var image = exactVariant?.Image;

        // Combinations
        var vm = new VariantListDto
        {           
            Variants = variants,
            TotalStock = variants.Sum(v => v.Quantity),
            Image = image,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            //MinPrice = exactVariant?.RegularPrice,
        };

        return vm;
    }
}
