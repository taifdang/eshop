using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Options.Commands.DeleteOptionValue;

public record DeleteOptionValueCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int OptionId { get; init; }
}

public class DeleteOptionValueCommandHandler : IRequestHandler<DeleteOptionValueCommand, Unit>
{
    private readonly IRepository<OptionValue> _optionValueRepository;
    public DeleteOptionValueCommandHandler(IRepository<OptionValue> optionValueRepository)
    {
        _optionValueRepository = optionValueRepository;
    }
    public async Task<Unit> Handle(DeleteOptionValueCommand request, CancellationToken cancellationToken)
    {
        var optionValue = await _optionValueRepository.FirstOrDefaultAsync(new OptionValueFilterSpec(request.Id, request.OptionId))
            ?? throw new EntityNotFoundException(nameof(OptionValue), request.Id);

        await _optionValueRepository.DeleteAsync(optionValue, cancellationToken);
        return Unit.Value;
    }
}
