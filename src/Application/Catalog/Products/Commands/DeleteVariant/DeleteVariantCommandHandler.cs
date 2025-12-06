using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.DeleteVariant;

public class DeleteVariantCommandHandler : IRequestHandler<DeleteVariantCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteVariantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = await _dbContext.Variants
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.ProductId == request.ProductId);

        Guard.Against.NotFound(request.Id, variant);

        _dbContext.Variants.Remove(variant);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
