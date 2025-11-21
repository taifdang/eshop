using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Commands.CreateVariant;

//Non option
public record CreateVariantCommand(Guid ProductId, decimal RegularPrice, int Quantity) : IRequest<Guid>;

public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, Guid>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;

    public CreateVariantCommandHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }
    public async Task<Guid> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = new ProductVariant
        {
            ProductId = request.ProductId,
            Price = request.RegularPrice,
            Quantity = request.Quantity,
            Status = Domain.Enums.IntentoryStatus.InStock,
            Percent = 0
        };

        await _productVariantRepository.AddAsync(variant, cancellationToken);

        return variant.ProductId;
    }
}
