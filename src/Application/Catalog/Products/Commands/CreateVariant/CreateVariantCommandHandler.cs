using Application.Catalog.Products.Services;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateVariant
{
    public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, Guid>
    {
        private readonly IProductService _productService;

        public CreateVariantCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Guid> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
            Guard.Against.NotFound(request.ProductId, product);

            var variant = new Variant
            {
                ProductId = request.ProductId,
                Price = request.RegularPrice,
                Quantity = request.Quantity,
                IsActive = true,
                IsDeleted = false
            };

            product.AddVariant(variant);

            await _productService.UpdateAsync(product, cancellationToken);

            return variant.ProductId;
        }
    }

}
