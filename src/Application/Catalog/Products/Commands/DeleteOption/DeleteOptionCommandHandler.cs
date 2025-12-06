using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.DeleteOption;

public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteOptionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
    {
        var productOption = await _dbContext.ProducOptions
            .FirstOrDefaultAsync(x => x.Id == request.OptionId && x.ProductId == request.ProductId);

        Guard.Against.NotFound(request.OptionId, productOption);

        _dbContext.ProducOptions.Remove(productOption);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}