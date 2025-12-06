using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.UpdateVariant;

public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateVariantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {

        var variant = await _dbContext.Variants.FirstOrDefaultAsync(x => x.Id == request.Id);
        Guard.Against.NotFound(request.Id, variant);

        variant.Price = request.RegularPrice;

        if (request.Quantity > 0)
        {
            variant.Quantity = request.Quantity;
        }
        else
        {
            throw new Exception("Quantity is not negative");
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
