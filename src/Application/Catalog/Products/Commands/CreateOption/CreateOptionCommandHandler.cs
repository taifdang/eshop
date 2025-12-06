using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.CreateOption;

public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateOptionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
    {
        var entityExist = await _dbContext.ProducOptions
            .AnyAsync(x => x.ProductId == request.ProductId && x.AllowImage, cancellationToken);

        if (entityExist && request.AllowImage)
        {
            throw new Exception("Only one product option can allow images per product.");
        }

        var productOption = new ProductOption
        {
            ProductId = request.ProductId,
            Name = request.OptionName,
            AllowImage = request.AllowImage,
        };

        _dbContext.ProducOptions.Add(productOption);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
