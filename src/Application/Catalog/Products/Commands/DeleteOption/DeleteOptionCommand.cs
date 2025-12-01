using MediatR;
using Domain.Entities;
using Ardalis.GuardClauses;
using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;

namespace Application.Catalog.Products.Commands.DeleteOption;

public record DeleteOptionCommand(Guid Id, Guid ProductId) : IRequest<Unit>;

public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, Unit>
{
    private readonly IRepository<ProductOption> _productOptionRepository;

    public DeleteOptionCommandHandler(IRepository<ProductOption> productOptionRepository)
    {
        _productOptionRepository = productOptionRepository;
    }

    public async Task<Unit> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
    {
        var spec = new ProductOptionSpec()
            .ByOptionId(request.Id)
            .ByProductId(request.ProductId);

        var productOption = await _productOptionRepository.FirstOrDefaultAsync(spec);
        Guard.Against.NotFound(request.Id, productOption);

        await _productOptionRepository.DeleteAsync(productOption, cancellationToken);

        return Unit.Value;
    }
}


