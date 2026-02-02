using Application.Abstractions;
using Application.Payment.Dtos;
using Contracts.IntegrationEvents;
using Domain.Repositories;
using MediatR;
using Outbox.Abstractions;
using System.Text.Json;

namespace Application.Payment.Commands.VerifyPaymentIpn;

public class VerifyPaymentIpnCommandHandler : IRequestHandler<VerifyPaymentIpnCommand, IpnResult>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IReadRepository<Domain.Entities.Order, Guid> _readRepository;
    private readonly IPollingOutboxMessageRepository _outboxRepository;

    public VerifyPaymentIpnCommandHandler(
        IPaymentGatewayFactory factory,
        IReadRepository<Domain.Entities.Order, Guid> readRepository,
        IPollingOutboxMessageRepository outboxRepository)
    {
        _factory = factory;
        _readRepository = readRepository;
        _outboxRepository = outboxRepository;
    }
    public async Task<IpnResult> Handle(VerifyPaymentIpnCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.Provider);
        var result = gateway.VerifyIpnCallback(request.Parameters);

        if(result.IsNullEvent)
        {
            return await Task.FromResult(new IpnResult
            {
                RspCode = "99",
                Message = "System error",
            });
        }

        if (!result.CheckSignature)
        {
            return await Task.FromResult(new IpnResult
            {
                RspCode = "97",
                Message = "Invalid signature",
            });
        }

        // get order from db
        var orderEntity = await _readRepository.FirstOrDefaultAsync(_readRepository.GetQueryableSet().Where(o => o.OrderNumber == result.OrderNumber));

        if (orderEntity == null)
        {
            return await Task.FromResult(new IpnResult
            {
                RspCode = "01",
                Message = "Order not found",
            });
        }

        if (orderEntity.TotalAmount.Amount != result.Amount)
        {
            return await Task.FromResult(new IpnResult
            {
                RspCode = "04",
                Message = "Invalid amount",
            });
        }

        // if order is already completed, no need to process again
        if (orderEntity.Status == Domain.Enums.OrderStatus.Completed)
        {
            return await Task.FromResult(new IpnResult
            {
                RspCode = "02",
                Message = "Order already processed",
            });
        }

        if (result.IsSuccess)
        {
            var integrationEvent = new PaymentSucceededIntegrationEvent
            {
                OrderNumber = result.OrderNumber,
                TransactionId = result.TransactionId,
                CardBrand = result.CardBrand
            };

            var message = new PollingOutboxMessage
            {
                CreateDate = DateTime.UtcNow,
                PayloadType = typeof(PaymentSucceededIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedDate = null
            };

            await _outboxRepository.AddAsync(message);
        }
        else
        {
            // when response code and transaction status are not success, create payment rejected event
            var integrationEvent = new PaymentRejectedIntegrationEvent
            {
                OrderNumber = result.OrderNumber,
                TransactionId = result.TransactionId,
                CardBrand = result.CardBrand
            };

            var message = new PollingOutboxMessage
            {
                CreateDate = DateTime.UtcNow,
                PayloadType = typeof(PaymentRejectedIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedDate = null
            };

            await _outboxRepository.AddAsync(message);
        }

        await _outboxRepository.SaveChangesAsync();

        return await Task.FromResult(new IpnResult
        {
            RspCode = "00",
            Message = "Confirm Success",
        });
    }
}
