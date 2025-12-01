using Application.Catalog.Products.Specifications;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Utilities;
using Ardalis.Specification;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.GenerateVariant;

public record GenerateVariantCommand(Guid ProductId, Dictionary<Guid, List<Guid>>? OptionValueFilter) : IRequest<Unit>;

public class GenerateVariantCommandHandler : IRequestHandler<GenerateVariantCommand, Unit>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<OptionValue> _optionValueRepository;

    public GenerateVariantCommandHandler(
        IRepository<ProductVariant> productVariantRepository,
        IRepository<Product> productRepository,
        IRepository<OptionValue> optionValueRepository)
    {
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
        _optionValueRepository = optionValueRepository;
    }

    public async Task<Unit> Handle(GenerateVariantCommand request, CancellationToken cancellationToken)
    {
        if (request.OptionValueFilter == null || request.OptionValueFilter.Count == 0)
            throw new ArgumentException("No option values provided.");

        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new EntityNotFoundException(nameof(Product), request.ProductId);

        // Implementation for generating variants would go here
        var optionValues = request.OptionValueFilter.SelectMany(x => x.Value).ToList();

        var optionValueSpec = new OptionValueSpec()
            .WithOptionValues(optionValues)
            .WithProjectionOf(new OptionValueProjectionSpec());

        var optionValueEntities = await _optionValueRepository.ListAsync(optionValueSpec, cancellationToken);

        // Logic to create variants based on optionValueEntities would be implemented here
        var optionValueDict = optionValueEntities.ToDictionary(x => x.Id, x => x.Value ?? x.Label );

        var combinations = CombinationHelper.CartesianProduct(request.OptionValueFilter.Values).ToList();
        if (combinations.Count == 0)
            throw new InvalidOperationException("No combinations generated.");

        const int batchSize = 20;
        var batch = new List<ProductVariant>(batchSize);
        foreach (var combo in combinations)
        {
            var ids = combo.ToList();

            var title = string.Join(" - ", ids.Select(id => optionValueDict[id]));

            var variant = new ProductVariant
            {
                ProductId = product.Id,
                Title = title,
                Percent = 0,
                Status = Domain.Enums.IntentoryStatus.InStock,
                VariantOptionValues = ids.Select(id => new VariantOptionValue { OptionValueId = id }).ToList()
            };

            batch.Add(variant);

            if (batch.Count >= batchSize)
            {
                await _productVariantRepository.AddRangeAsync(batch, cancellationToken);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
            await _productVariantRepository.AddRangeAsync(batch, cancellationToken);

        return Unit.Value;
    }
}

