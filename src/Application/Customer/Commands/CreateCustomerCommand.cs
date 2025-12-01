using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Customer.Commands;

public record CreateCustomerCommand(Guid UserId, string Email) : IRequest<Unit>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
{
    private readonly IRepository<Domain.Entities.Customer> _customerRepository;

    public CreateCustomerCommandHandler(IRepository<Domain.Entities.Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Domain.Entities.Customer
        {
            UserId = request.UserId,
            Email = request.Email,
        };

        await _customerRepository.AddAsync(customer, cancellationToken);

        return Unit.Value;
    }
}
