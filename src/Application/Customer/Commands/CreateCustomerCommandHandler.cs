using Domain.Repositories;
using MediatR;

namespace Application.Customer.Commands;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }   

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Domain.Entities.Customer
        {
            UserId = request.UserId,
            Email = request.Email,
        };

        await _repository.AddAsync(customer);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
