using Application.Common.Interfaces.Persistence;
using Application.Customer.Specifications;
using Ardalis.Specification;
using MediatR;

namespace Application.Customer.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery(Guid UserId) : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByUserIdQuery, CustomerDto>
{
    private readonly IReadRepository<Domain.Entities.Customer> _customerRepo;

    public GetCustomerByIdQueryHandler(IReadRepository<Domain.Entities.Customer> customerRepo)
    {
        _customerRepo = customerRepo;
    }
    public async Task<CustomerDto> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new CustomerSpec().ByUserId(request.UserId).WithProjectionOf(new CustomerProjectionSpec());

        var customer = await _customerRepo.FirstOrDefaultAsync(spec);

        return customer;
    }
}
