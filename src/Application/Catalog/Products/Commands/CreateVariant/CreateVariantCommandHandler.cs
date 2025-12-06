using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateVariant
{
    public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateVariantCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Handle(CreateVariantCommand request, CancellationToken cancellationToken)
        {
            var variant = new Variant
            {
                ProductId = request.ProductId,
                Price = request.RegularPrice,
                Quantity = request.Quantity,
                IsActive = true,
                IsDeleted = false
            };

            _dbContext.Variants.Add(variant);
            await _dbContext.SaveChangesAsync(cancellationToken);   

            return variant.ProductId;
        }
    }

}
