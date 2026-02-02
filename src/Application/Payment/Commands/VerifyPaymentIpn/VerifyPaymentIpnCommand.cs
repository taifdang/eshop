using Application.Payment.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Payment.Commands.VerifyPaymentIpn;

public record VerifyPaymentIpnCommand(
    PaymentProvider Provider,
    IDictionary<string, string> Parameters) 
    : IRequest<IpnResult>;
