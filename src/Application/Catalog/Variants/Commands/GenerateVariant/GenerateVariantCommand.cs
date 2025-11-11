using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Application.Common.Utilities;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Commands.GenerateVariant;

public record GenerateVariantCommand : IRequest<Unit>
{
    public int ProductId { get; init; } // product id
    public Dictionary<string, List<int>> OptionValueMap { get; init; } = new(); //ex: { "Color": [1,2], "Size": [3,4] }
}

public class GenerateVariantCommandHandler : IRequestHandler<GenerateVariantCommand, Unit>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    private readonly IRepository<Domain.Entities.Product> _productRepository;
    private readonly IRepository<OptionValue> _optionValueRepository;

    public GenerateVariantCommandHandler(
        IRepository<ProductVariant> productVariantRepository,
        IRepository<Domain.Entities.Product> productRepository,
        IRepository<OptionValue> optionValueRepository)
    {
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
        _optionValueRepository = optionValueRepository;
    }

    public async Task<Unit> Handle(GenerateVariantCommand request, CancellationToken cancellationToken)
    {
        if (request.OptionValueMap == null || request.OptionValueMap.Count == 0)
            throw new ArgumentException("No option values provided.");

        var product = await _productRepository.FirstOrDefaultAsync(new ProductFilterSpec(request.ProductId), cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Product), request.ProductId);

        // Implementation for generating variants would go here
        var optionValues = request.OptionValueMap.SelectMany(x => x.Value).ToList();

        var optionValueEntities = await _optionValueRepository.ListAsync(new OptionValueExistSpec(optionValues), cancellationToken);

        // Logic to create variants based on optionValueEntities would be implemented here
        var optionValueDict = optionValueEntities.ToDictionary(x => x.Id, x => x.Label ?? x.Value );

        //var combinations = request.OptionValueMap.Values.ToList();
        var combinations = CombinationHelper.CartesianProduct(request.OptionValueMap.Values).ToList();
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

