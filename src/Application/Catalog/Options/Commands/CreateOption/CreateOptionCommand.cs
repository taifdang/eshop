using Application.Common.Interfaces;
using Application.Common.Specifications;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Options.Commands.CreateOption;

public record CreateOptionCommand : IRequest<Unit>
{
    public int ProductId { get; init; }
    public string OptionName { get; init; }
    public bool AllowImage { get; init; } = false;
}

public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, Unit>
{
    private readonly IRepository<ProductOption> _productOptionRepository;
    private readonly IMapper _mapper;
    public CreateOptionCommandHandler(IRepository<ProductOption> productOptionrepository, IMapper mapper)
    {
        _productOptionRepository = productOptionrepository;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
    {
        var existing = await _productOptionRepository.AnyAsync(new OptionImageFilterSpec(request.ProductId));

        if (existing && request.AllowImage)
        {
            throw new Exception("Only one product option can allow images per product.");
        }

        var productOption = _mapper.Map<ProductOption>(request);

        await _productOptionRepository.AddAsync(productOption, cancellationToken);
        return Unit.Value;
    }
}