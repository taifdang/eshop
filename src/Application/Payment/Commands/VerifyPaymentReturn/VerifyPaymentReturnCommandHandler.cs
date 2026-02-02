using Application.Abstractions;
using Application.Payment.Dtos;
using MediatR;

namespace Application.Payment.Commands.VerifyPaymentReturn;

public class VerifyPaymentReturnCommandHandler : IRequestHandler<VerifyPaymentReturnCommand, PaymentReturnResult>
{
    private readonly IPaymentGatewayFactory _factory;

    public VerifyPaymentReturnCommandHandler(IPaymentGatewayFactory factory)
    {
        _factory = factory;
    }
    public Task<PaymentReturnResult> Handle(VerifyPaymentReturnCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.Provider);
        var result = gateway.VerifyReturnUrl(request.Parameters);

        if (!result.CheckSignature)
        {
            return Task.FromResult(new PaymentReturnResult
            {
                IsValid = false,
                RspCode = "97",
                Message = "Invalid signature",
            });
        }

        if (result.IsSuccess)
        {
            return Task.FromResult(new PaymentReturnResult
            {
                IsValid = true,
                RspCode = "00",
                Message = "Payment verified successfully"
            });
        }

        return Task.FromResult(new PaymentReturnResult
        {
            IsValid = false,
            RspCode = result.ResCode,
            Message = "Invalid signature",
        });   
    }
}
