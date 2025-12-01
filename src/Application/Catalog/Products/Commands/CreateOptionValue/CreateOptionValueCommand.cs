using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public record CreateOptionValueCommand(Guid OptionId, string Value, string? Label) : IRequest<Unit>;

public class CreateOptionValueCommandHandler : IRequestHandler<CreateOptionValueCommand, Unit>
{
    private readonly IRepository<OptionValue> _optionValueRepository;

    public CreateOptionValueCommandHandler(IRepository<OptionValue> optionValueRepository)
    {
        _optionValueRepository = optionValueRepository;
    }

    public async Task<Unit> Handle(CreateOptionValueCommand request, CancellationToken cancellationToken)
    {
        var spec = new OptionValueSpec()
            .ByOptionId(request.OptionId);

        var optionExist = await _optionValueRepository.AnyAsync(spec, cancellationToken);
        Guard.Against.NotFound(request.OptionId, optionExist);

        var optionValue = new OptionValue
        {
            ProductOptionId = request.OptionId,
            Value = request.Value,
            Label = request.Label,         
        };

        await _optionValueRepository.AddAsync(optionValue, cancellationToken);

        return Unit.Value;
    }
}
