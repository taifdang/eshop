using Application.Customer.Dtos;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Customer.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery(Guid UserId) : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByUserIdQuery, CustomerDto>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdQueryHandler(
        IMapper mapper,
        ICustomerRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<CustomerDto> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.FirstOrDefaultAsync(_repository.GetQueryableSet().Where(x => x.UserId == request.UserId));
        return _mapper.Map<CustomerDto>(customer);
    }
}
