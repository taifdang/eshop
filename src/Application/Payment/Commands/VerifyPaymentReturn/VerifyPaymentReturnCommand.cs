using Application.Payment.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Payment.Commands.VerifyPaymentReturn;

public record VerifyPaymentReturnCommand(
    PaymentProvider Provider, 
    IDictionary<string, string> Parameters) 
    : IRequest<PaymentReturnResult>;
