using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Payment.Commands.CreatePaymentUrl;

public class CreatePaymentUrlCommandHandler : IRequestHandler<CreatePaymentUrlCommand, CreatePaymentUrlResult>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IApplicationDbContext _context;

    public CreatePaymentUrlCommandHandler(
        IPaymentGatewayFactory factory,
        IApplicationDbContext context)
    {
        _factory = factory;
        _context = context;
    }

    public Task<CreatePaymentUrlResult> Handle(CreatePaymentUrlCommand request, CancellationToken cancellationToken)
    {
        // warn: 
        var gateway = _factory.Resolve(request.Provider);

        // get order from db
        var orderEntity = _context.Orders.FirstOrDefault(o => o.OrderNumber == request.OrderNumber);

        if (orderEntity == null)
        {
            return Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Order not found"
            });
        }

        if (orderEntity.TotalAmount.Amount != request.Amount)
        {
            return Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Invalid amount"
            });
        }

        // if order is already completed, no need to process again
        if (orderEntity.Status == Domain.Enums.OrderStatus.Completed)
        {
            return Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = "Order already processed"
            });
        }

        return gateway.CreatePaymentUrl(new CreatePaymentUrlRequest
        {
            OrderNumber = request.OrderNumber,
            Amount = request.Amount,
            OrderDate = request.OrderDate,
        });
    }
}
