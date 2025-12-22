using Application.Common.Interfaces;
using MediatR;

namespace Application.Customer.Commands;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateCustomerCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Domain.Entities.Customer
        {
            UserId = request.UserId,
            Email = request.Email,
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
