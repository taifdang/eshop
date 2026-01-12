using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalServices.Payments.Vnpay;

public class VnpayPaymentGateway : IPaymentGateway
{
    private readonly VnpayOptions _vnpOption;
    private readonly ICurrentIPAddressProvider _currentIPAddressProvider;

    public VnpayPaymentGateway(
        IOptions<VnpayOptions> options,
        ICurrentIPAddressProvider currentIPAddressProvider)
    {
        _vnpOption = options.Value;
        _currentIPAddressProvider = currentIPAddressProvider;
    }

    public Task<CreatePaymentUrlResult> CreatePaymentUrl(CreatePaymentUrlRequest request)
    {
        try
        {
            // Here: implement the logic to create payment URL for Vnpay
            var paymentUrl = GetEncodeUrl(request.OrderNumber, request.Amount, request.OrderDate);

            return Task.FromResult(new CreatePaymentUrlResult
            {
                Status = true,
                Data = paymentUrl,
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CreatePaymentUrlResult
            {
                Status = false,
                Error = ex.Message.ToString(),
            });
        }
    }

    public VerifyReturnUrlResult VerifyReturnUrl(IDictionary<string, string> parameters)
    {
        // Here: implement the logic to verify the return URL from Vnpay using the parameters dictionary
        // no process business logic, just verify signature and return result and display ui
        string hashSecret = _vnpOption.HashSecret;

        bool checkSignature = VnpaySignatureHelper.VerifySignature(parameters, hashSecret);

        return new VerifyReturnUrlResult
        {
            CheckSignature = checkSignature,
            IsSuccess = parameters["vnp_ResponseCode"] == "00" && parameters["vnp_TransactionStatus"] == "00",
            ResCode = parameters["vnp_ResponseCode"],          
        };
    }

    public VerifyIpnResult VerifyIpnCallback(IDictionary<string, string> parameters)
    {
        // Here: implement the logic to verify the IPN callback from Vnpay using the parameters dictionary
        string hashSecret = _vnpOption.HashSecret;
        bool checkSignature = VnpaySignatureHelper.VerifySignature(parameters, hashSecret);

        return new VerifyIpnResult
        {
            CheckSignature = checkSignature,  // validate signature and response code
            IsSuccess = parameters["vnp_ResponseCode"] == "00" && parameters["vnp_TransactionStatus"] == "00",
            ResCode = parameters["vnp_ResponseCode"],
            OrderNumber = long.Parse(parameters["vnp_TxnRef"]),
            TransactionId = parameters["vnp_TransactionNo"],
            Amount = decimal.Parse(parameters["vnp_Amount"]) / 100,
            CardBrand = parameters["vnp_CardType"],
            IsNullEvent = parameters.Count <= 0
        };

    }

#if (deprecated)
    // Return URL verification

    public Task<PaymentReturnResult> VerifyReturnUrl(IDictionary<string, string> parameters)
    {
        // Here: implement the logic to verify the return URL from Vnpay using the parameters dictionary
        // no process business logic, just verify signature and return result and display ui
        string hashSecret = _vnpOption.HashSecret;

        bool checkSignature = VnpaySignatureHelper.VerifySignature(parameters, hashSecret);

        if (checkSignature)
        {
            if (parameters["vnp_ResponseCode"] == "00" && parameters["vnp_TransactionStatus"] == "00")
            {
                return Task.FromResult(new PaymentReturnResult
                {
                    Valid = true,
                    ResponseCode = "00",
                    Message = "Payment verified successfully",
                });
            }
            else
            {
                return Task.FromResult(new PaymentReturnResult
                {
                    Valid = false,
                    ResponseCode = parameters["vnp_ResponseCode"],
                    Message = "Invalid signature",
                });
            }
        }
        else
        {
            return Task.FromResult(new PaymentReturnResult
            {
                Valid = false,
                ResponseCode = parameters["vnp_ResponseCode"],
                Message = "Oops! Something went wrong!",
            });
        }
    }

    // IPN: Instant Payment Notification 

    public Task<IpnResult> VerifyIpnCallback(IDictionary<string, string> parameters)
    {     
        try
        {
            // Here: implement the logic to verify the IPN callback from Vnpay using the parameters dictionary
            string hashSecret = _vnpOption.HashSecret;
            bool checkSignature = VnpaySignatureHelper.VerifySignature(parameters, hashSecret);

            // validate signature and response code
            if (!checkSignature)
            {
                return Task.FromResult(new IpnResult
                {
                    RspCode = "97",
                    Message = "Invalid signature",
                });
            }

            // destructure parameters
            var orderNumber = long.Parse(parameters["vnp_TxnRef"]);
            var amount = decimal.Parse(parameters["vnp_Amount"]) / 100;
            var transactionId = parameters["vnp_TransactionNo"];
            var cardBrand = parameters.ContainsKey("vnp_CardType") ? parameters["vnp_CardType"] : "N/A";
            var responseCode = parameters["vnp_ResponseCode"];
            var transactionStatus = parameters["vnp_TransactionStatus"];
            
            // get order from db
            var orderEntity = _context.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);

            // not found order
            if (orderEntity == null)
            {
                return Task.FromResult(new IpnResult
                {
                    RspCode = "01",
                    Message = "Order not found",
                });
            }

            // validate amount
            if (orderEntity.TotalAmount.Amount != amount)
            {
                return Task.FromResult(new IpnResult
                {
                    RspCode = "04",
                    Message = "Invalid amount",
                });
            }

            // if order is already completed, no need to process again
            if (orderEntity.Status == Domain.Enums.OrderStatus.Completed)
            {
                return Task.FromResult(new IpnResult
                {
                    RspCode = "02",
                    Message = "Order already processed",
                });
            }

            // then, create integration event
            if(responseCode == "00" && transactionStatus == "00")
            {
                var integrationEvent = new PaymentSucceededIntegrationEvent
                {
                    OrderNumber = orderNumber,
                    TransactionId = transactionId,
                    CardBrand = cardBrand
                };

                var message = new PollingOutboxMessage
                {
                    CreateDate = DateTime.UtcNow,
                    PayloadType = typeof(PaymentSucceededIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                    Payload = JsonSerializer.Serialize(integrationEvent),
                    ProcessedDate = null
                };

                _outboxRepository.AddAsync(message);
            }
            else
            {
                // when response code and transaction status are not success, create payment rejected event
                var integrationEvent = new PaymentRejectedIntegrationEvent
                {
                    OrderNumber = orderNumber,
                    TransactionId = transactionId,
                    CardBrand = cardBrand
                };

                var message = new PollingOutboxMessage
                {
                    CreateDate = DateTime.UtcNow,
                    PayloadType = typeof(PaymentRejectedIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                    Payload = JsonSerializer.Serialize(integrationEvent),
                    ProcessedDate = null
                };

                _outboxRepository.AddAsync(message);
            }


            _outboxRepository.SaveChangesAsync();

            return Task.FromResult(new IpnResult
            {
                RspCode = "00",
                Message = "Confirm Success",
            });

        }
        catch(Exception ex)
        {
            return Task.FromResult(new IpnResult
            {
                RspCode = "99",
                Message = "System error",
            });
        }
    }
#endif
    public string GetEncodeUrl(long OrderNumber, decimal Amount, DateTime OrderDate)
    {
        SortedDictionary<string, string> dict = new SortedDictionary<string, string>
        {
            {"vnp_Amount", (Amount * 100).ToString()},
            {"vnp_Command", _vnpOption.Command},
            {"vnp_CreateDate", OrderDate.ToString("yyyyMMddHHmmss")},
            {"vnp_CurrCode", _vnpOption.CurrCode},
            // warn: when use behind proxy, this ip will be proxy ip, not client ip
            // need to get real client ip from header
            {"vnp_IpAddr", _currentIPAddressProvider.GetCurrentIPAddress() ?? ""},
            {"vnp_Locale", _vnpOption.Locale},
            {"vnp_OrderInfo", $"Payment for {OrderNumber.ToString()} with amount {Amount.ToString()}"},
            {"vnp_OrderType", _vnpOption.OrderType},
            {"vnp_ReturnUrl", _vnpOption.ReturnUrl},
            {"vnp_TmnCode", _vnpOption.TmnCode},
            {"vnp_TxnRef", OrderNumber.ToString()},
            {"vnp_Version", _vnpOption.Version},
        };

        // encode url string
        string query = VnpaySignatureHelper.BuildRawData(dict);

        string secureHash = HmacHelper.ComputeHmacSha512(query, _vnpOption.HashSecret);

        return $"{_vnpOption.BaseUrl}?{query}&vnp_SecureHash={secureHash}";
    }
}
