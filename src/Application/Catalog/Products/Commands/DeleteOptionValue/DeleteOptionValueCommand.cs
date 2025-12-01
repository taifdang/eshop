using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteOptionValue;

public record DeleteOptionValueCommand(Guid OptionValueId, Guid OptionId) : IRequest<Unit>;

public class DeleteOptionValueCommandHandler : IRequestHandler<DeleteOptionValueCommand, Unit>
{
    private readonly IRepository<OptionValue> _optionValueRepository;

    public DeleteOptionValueCommandHandler(IRepository<OptionValue> optionValueRepository)
    {
        _optionValueRepository = optionValueRepository;
    }

    public async Task<Unit> Handle(DeleteOptionValueCommand request, CancellationToken cancellationToken)
    {
        var spec = new OptionValueSpec()
            .ByOptionId(request.OptionId)
            .ByOptionValueId(request.OptionValueId);

        var optionValue = await _optionValueRepository.FirstOrDefaultAsync(spec);
        Guard.Against.NotFound(request.OptionValueId, optionValue);

        await _optionValueRepository.DeleteAsync(optionValue, cancellationToken);

        return Unit.Value;
    }
}
