using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.BulkUpdateVariant;

public class BulkUpdateVariantCommandHandler : IRequestHandler<BulkUpdateVariantCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public BulkUpdateVariantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(BulkUpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var variants = await _dbContext.Variants.Where(x => x.ProductId == request.ProductId).ToListAsync();
        Guard.Against.NotFound(request.ProductId, variants);

        foreach (var v in variants)
        {
            if (request.Price.HasValue) v.Price = request.Price.Value;
            if (request.Quantity.HasValue) v.Quantity = request.Quantity.Value;
            if (!string.IsNullOrWhiteSpace(request.Sku)) v.Sku = request.Sku;
            if (request.IsActive != v.IsActive) v.IsActive = request.IsActive;
        }

        _dbContext.Variants.UpdateRange(variants);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return request.ProductId;
    }
}
