using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utilities;
using Ardalis.Specification;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.GenerateVariant;

public record GenerateVariantCommand(Guid ProductId, Dictionary<Guid, List<Guid>>? OptionValueFilter) : IRequest<Unit>;

public class GenerateVariantCommandHandler : IRequestHandler<GenerateVariantCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public GenerateVariantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(GenerateVariantCommand request, CancellationToken cancellationToken)
    {
        if (request.OptionValueFilter == null || request.OptionValueFilter.Count == 0)
            throw new ArgumentException("No option values provided.");

        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Product), request.ProductId);

        // Implementation for generating variants would go here
        var optionValues = request.OptionValueFilter.SelectMany(x => x.Value).ToList();

        //var optionValueSpec = new OptionValueSpec()
        //    .WithOptionValues(optionValues)
        //    .WithProjectionOf(new OptionValueProjectionSpec());

        var query = _dbContext.OptionValues.AsQueryable();

        query = query.Where(m => optionValues.Contains(m.Id));

        var optionValueEntities = await query.Select(m => new OptionValueDto(m.Id, m.Value)).ToListAsync(cancellationToken);

        // Logic to create variants based on optionValueEntities would be implemented here
        var optionValueDict = optionValueEntities.ToDictionary(x => x.Id, x => x.Value);

        var combinations = CombinationHelper.CartesianProduct(request.OptionValueFilter.Values).ToList();

        if (combinations.Count == 0)
            throw new InvalidOperationException("No combinations generated.");

        const int batchSize = 20;
        var batch = new List<Variant>(batchSize);

        try
        {
            foreach (var combo in combinations)
            {
                var ids = combo.ToList();

                var title = string.Join(" - ", ids.Select(id => optionValueDict[id]));

                var variant = new Variant
                {
                    ProductId = product.Id,
                    Title = title,
                    VariantOptions = ids.Select(id => new VariantOption { OptionValueId = id }).ToList()
                };

                batch.Add(variant);

                if (batch.Count >= batchSize)
                {
                    await _dbContext.Variants.AddRangeAsync(batch, cancellationToken);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                await _dbContext.Variants.AddRangeAsync(batch, cancellationToken);
            }
               
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while generating variants.", ex);
        }

        return Unit.Value;
    }
}

