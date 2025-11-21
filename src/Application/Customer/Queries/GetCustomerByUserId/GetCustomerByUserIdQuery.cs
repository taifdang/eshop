using Application.Common.Interfaces;
using Application.Common.Specifications;
using MediatR;

namespace Application.Customer.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery(Guid UserId) : IRequest<Guid>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByUserIdQuery, Guid>
{
    private readonly IReadRepository<Domain.Entities.Customer> _customerRepo;
    public GetCustomerByIdQueryHandler(IReadRepository<Domain.Entities.Customer> customerRepo)
    {
        _customerRepo = customerRepo;
    }
    public async Task<Guid> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new CustomerByUserIdSpec(request.UserId);

        var customer = await _customerRepo.FirstOrDefaultAsync(spec);

        return customer.Id;
    }
}
