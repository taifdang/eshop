using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Options.Commands.CreateOptionValue;

public record CreateOptionValueCommand : IRequest<Unit>
{
    public int ProductOptionId { get; init; }
    public string Value { get; init; } 
    public string? Label { get; init; }
}

public class CreateOptionValueCommandHandler : IRequestHandler<CreateOptionValueCommand, Unit>
{
    //private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<OptionValue> _optionValueRepository;
    private readonly IMapper _mapper;
    public CreateOptionValueCommandHandler(IRepository<OptionValue> optionValueRepository, IMapper mapper)
    {
        _optionValueRepository = optionValueRepository;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(CreateOptionValueCommand request, CancellationToken cancellationToken)
    {
        var option = await _optionValueRepository.FirstOrDefaultAsync(new OptionValueFilterSpec(null, request.ProductOptionId))
                ?? throw new EntityNotFoundException(nameof(ProductOption), request.ProductOptionId);

        var optionValue = _mapper.Map<OptionValue>(request);

        await _optionValueRepository.AddAsync(optionValue, cancellationToken);
        return Unit.Value;
    }
}
