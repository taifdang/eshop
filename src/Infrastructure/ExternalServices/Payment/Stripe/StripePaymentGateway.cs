using Application.Abstractions;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Infrastructure.ExternalServices.Payment.Stripe;

public class StripePaymentGateway : IPaymentGateway
{
    private readonly StripeOptions _options;

    public StripePaymentGateway(IOptions<StripeOptions> options)
    {
        _options = options.Value;
        StripeConfiguration.ApiKey = _options.SecretKey;
    }

    public async Task<CreatePaymentUrlResult> CreatePaymentUrl(CreatePaymentUrlRequest request)
    {
        try
        {
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = request.Amount,
                            Currency = _options.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order {request.OrderNumber}"
                            }
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                // NOTE: Returning a URL with the orderNumber parameter is not a good idea and is not safe.
                SuccessUrl = $"{_options.ReturnUrl}?orderNumber={request.OrderNumber}",
                CancelUrl = $"{_options.ReturnUrl}?orderNumber={request.OrderNumber}&cancelled=true",
            };

            // Attach order number to session metadata so webhook can identify the order
            sessionOptions.Metadata = new Dictionary<string, string>
            {
                { "order_number", request.OrderNumber.ToString() }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(sessionOptions);

            return new CreatePaymentUrlResult
            {
                Status = true,
                Data = session.Url
            };
        }
        catch (Exception ex)
        {
            return new CreatePaymentUrlResult
            {
                Status = false,
                Error = ex.Message
            };
        }
    }

    public VerifyReturnUrlResult VerifyReturnUrl(IDictionary<string, string> parameters)
    {
        // Stripe does not sign return URLs. Verification should be done via webhooks.
        return new VerifyReturnUrlResult
        {
            CheckSignature = false,
            IsSuccess = false,
            ResCode = string.Empty
        };
    }

    public VerifyIpnResult VerifyIpnCallback(IDictionary<string, string> parameters)
    {
        // Expecting parameters to include raw payload and signature header when called from controller for Stripe webhooks.
        // payload: raw JSON body
        // sig_header: value of Stripe-Signature header
        if (!parameters.ContainsKey("payload") || !parameters.ContainsKey("sig_header"))
        {
            return new VerifyIpnResult
            {
                CheckSignature = false,
                IsSuccess = false,
                IsNullEvent = true,
                ResCode = string.Empty,
                OrderNumber = 0,
                TransactionId = string.Empty,
                Amount = 0m
            };
        }

        var payload = parameters["payload"];
        var sigHeader = parameters["sig_header"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(payload, sigHeader, _options.WebhookSecret);

            // handle checkout.session.completed and payment_intent.succeeded
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null)
                {
                    return new VerifyIpnResult { CheckSignature = true, IsNullEvent = true, IsSuccess = false, ResCode = string.Empty };
                }

                long orderNumber = 0;
                if (session.Metadata != null && session.Metadata.ContainsKey("order_number"))
                {
                    long.TryParse(session.Metadata["order_number"], out orderNumber);
                }

                decimal amount = Convert.ToDecimal(session.AmountTotal) / 100m;
                var transactionId = session.PaymentIntentId ?? string.Empty;

                return new VerifyIpnResult
                {
                    CheckSignature = true,
                    IsSuccess = session.PaymentStatus == "paid",
                    IsNullEvent = false,
                    ResCode = string.Empty,
                    OrderNumber = orderNumber,
                    TransactionId = transactionId,
                    Amount = amount,
                    CardBrand = null
                };
            }
            else if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var pi = stripeEvent.Data.Object as PaymentIntent;
                if (pi == null)
                {
                    return new VerifyIpnResult { CheckSignature = true, IsNullEvent = true, IsSuccess = false, ResCode = string.Empty };
                }

                long orderNumber = 0;
                if (pi.Metadata != null && pi.Metadata.ContainsKey("order_number"))
                {
                    long.TryParse(pi.Metadata["order_number"], out orderNumber);
                }

                decimal amount = Convert.ToDecimal(pi.Amount) / 100m;

                return new VerifyIpnResult
                {
                    CheckSignature = true,
                    IsSuccess = true,
                    IsNullEvent = false,
                    ResCode = string.Empty,
                    OrderNumber = orderNumber,
                    TransactionId = pi.Id ?? string.Empty,
                    Amount = amount,
                    CardBrand = null
                };
            }

            return new VerifyIpnResult { CheckSignature = true, IsNullEvent = true, IsSuccess = false, ResCode = string.Empty };
        }
        catch (StripeException)
        {
            return new VerifyIpnResult { CheckSignature = false, IsNullEvent = false, IsSuccess = false, ResCode = string.Empty };
        }
        catch (Exception)
        {
            return new VerifyIpnResult { CheckSignature = false, IsNullEvent = false, IsSuccess = false, ResCode = string.Empty };
        }
    }
}
