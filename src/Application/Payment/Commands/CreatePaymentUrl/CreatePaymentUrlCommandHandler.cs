using Application.Abstractions;
using Domain.Repositories;
using MediatR;

namespace Application.Payment.Commands.CreatePaymentUrl;

public class CreatePaymentUrlCommandHandler : IRequestHandler<CreatePaymentUrlCommand, CreatePaymentUrlResult>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IReadRepository<Domain.Entities.Order, Guid> _readRepository;

    public CreatePaymentUrlCommandHandler(
        IPaymentGatewayFactory factory,
        IReadRepository<Domain.Entities.Order, Guid> readRepository)
    {
        _factory = factory;
        _readRepository = readRepository;
    }

    public async Task<CreatePaymentUrlResult> Handle(CreatePaymentUrlCommand request, CancellationToken cancellationToken)
    {
        // warn: 
        var gateway = _factory.Resolve(request.Provider);

        // get order from db
        var orderEntity = await _readRepository.FirstOrDefaultAsync(_readRepository.GetQueryableSet().Where(o => o.OrderNumber == request.OrderNumber));

        if (orderEntity == null)
        {
            return await Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Order not found"
            });
        }

        if (orderEntity.TotalAmount.Amount != request.Amount)
        {
            return await Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Invalid amount"
            });
        }

        // if order is already completed, no need to process again
        if (orderEntity.Status == Domain.Enums.OrderStatus.Completed)
        {
            return await Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Order already processed"
            });
        }

        return await gateway.CreatePaymentUrl(new CreatePaymentUrlRequest
        {
            OrderNumber = request.OrderNumber,
            Amount = request.Amount,
            OrderDate = request.OrderDate,
        });
    }
}
