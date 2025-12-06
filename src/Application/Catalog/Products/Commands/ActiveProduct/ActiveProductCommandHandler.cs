using Application.Common.Interfaces;
using MediatR;

namespace Application.Catalog.Products.Commands.ActiveProduct;

public class ActiveProductCommandHandler : IRequestHandler<ActiveProductCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public ActiveProductCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ActiveProductCommand request, CancellationToken cancellationToken)
    {
        var product = _dbContext.Products.Find(request.ProductId);
        if (product == null)
        {
            throw new Exception($"Product with ID {request.ProductId} not found.");
        }
        product.IsActive = request.IsActive;
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
