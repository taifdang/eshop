using Application.Common.Interfaces;
using Application.Common.Specifications;
using Application.Customer.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Customer.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery : IRequest<CustomerDto>
{
    public Guid UserId { get; set; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByUserIdQuery, CustomerDto>
{
    private readonly IReadRepository<Domain.Entities.Customer> _customerRepository;
    private readonly IMapper _mapper;
    public GetCustomerByIdQueryHandler(IReadRepository<Domain.Entities.Customer> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
    public async Task<CustomerDto> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(new CustomerUserSpec(request.UserId), cancellationToken);
        return customer;
    }
}
