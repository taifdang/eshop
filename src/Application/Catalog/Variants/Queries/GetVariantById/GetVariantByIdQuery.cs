using Application.Catalog.Variants.Dtos;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Queries.GetVariantById;

public record GetVariantByIdQuery : IRequest<VariantDto>
{
    public int Id { get; init; }
}

public class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    public GetVariantByIdQueryHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }
    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new VariantWithOptionSpec(request.Id);
        var variantVm = await _productVariantRepository.FirstOrDefaultAsync(specification, cancellationToken);
        return variantVm;
    }
}
