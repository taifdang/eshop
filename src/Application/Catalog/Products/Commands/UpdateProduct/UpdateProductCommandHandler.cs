using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateProductCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(request.Id);
        Guard.Against.NotFound(request.Id, product);

        product.CategoryId = request.CategoryId;
        product.Name = request.Title;
        product.Description = request.Description;

        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
