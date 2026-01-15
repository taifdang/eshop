using Api.Models.Requests;
using Application.Payment.Commands.CreatePaymentUrl;
using Application.Payment.Commands.VerifyPaymentIpn;
using Application.Payment.Commands.VerifyPaymentReturn;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class PaymentApi
{
    public static IEndpointRouteBuilder MapPaymentApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGroup("api/v1/payment")
            .MapPaymentApi()
            .WithTags("Payment Api");

        return builder;
    }
    public static RouteGroupBuilder MapPaymentApi(this RouteGroupBuilder group)
    {
        group.MapPost("/create",
            async (IMediator mediator, [FromBody] CreatePaymentUrlRequestDto request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new CreatePaymentUrlCommand(request.OrderNumber, request.Amount, request.Provider, DateTime.UtcNow), cancellationToken);
                return result;
            })
            .WithName("CreatePayment")
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost($"/verify-return/{{provider}}",
            async (IMediator mediator, [FromRoute] string provider, [FromBody] Dictionary<string, string> request, CancellationToken cancellationToken) =>
            {
                var paymentProvider = PaymentProviderMapper.From(provider);
                var result = await mediator.Send(new VerifyPaymentReturnCommand(paymentProvider, request), cancellationToken);
                return result;
            })
            .WithName("ReturnUrlVerify")
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost($"/ipn/{{provider}}",
            async (IMediator mediator, [FromRoute] string provider, [FromBody] Dictionary<string, string> request, CancellationToken cancellationToken) =>
            {
                var paymentProvider = PaymentProviderMapper.From(provider);
                var result = await mediator.Send(new VerifyPaymentIpnCommand(paymentProvider, request), cancellationToken);
                return result;
            })
            .WithName("IpnCallback")
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }

    public static class PaymentProviderMapper
    {
        public static PaymentProvider From(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
                throw new ArgumentException("Provider is required");

            return provider.ToLowerInvariant() switch
            {
                "vnpay" => PaymentProvider.Vnpay,
                "stripe" => PaymentProvider.Stripe,
                _ => throw new NotSupportedException($"Payment provider '{provider}' is not supported")
            };
        }
    }
}
