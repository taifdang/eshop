using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Payments;
using Outbox.Abstractions;
namespace Infrastructure.ExternalServices;

public class PaymentService
{
    private readonly PaymentGatewayFactory _factory;
    private readonly ApplicationDbContext _context;
    private readonly IPollingOutboxMessageRepository _pollingOutboxMessageRepository;

    public PaymentService(
        PaymentGatewayFactory factory,
        ApplicationDbContext db,
        IPollingOutboxMessageRepository pollingOutboxMessageRepository)
    {
        _factory = factory;
        _context = db;
        _pollingOutboxMessageRepository = pollingOutboxMessageRepository;
    }
    public Task<PaymentResult> CreatePaymentAsync(CreatePaymentRequest request)
    {
        var url = $"https://paypal.vn/pay?order={request.OrderNumber}";
        return Task.FromResult(new PaymentResult(url, DateTime.UtcNow.Ticks.ToString()));
    }

    public Task<bool> HandleCallbackAsync(IDictionary<string, string> queryParams)
    {
        // validate

        return Task.FromResult(true);
    }
    //public async Task<string> CreatePaymentAsync(CreatePaymentRequest request)
    //{
    //    var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);
    //    var provider = _factory.GetPaymentProvider(request.Provider);

    //    var result = provider.CreatePaymentAsync(request);

    //    order.AddPayment(request.Provider, result.Result.PaymentUrl, result.Result.TransactionId);
    //    await _context.SaveChangesAsync();

    //    return result.Result.PaymentUrl;
    //}

    //public async Task<bool> HandleCallbackAsync(PaymentProvider provider, IDictionary<string, string> data)
    //{
    //    var paymentProvider = _factory.GetPaymentProvider(provider);
    //    var success = await paymentProvider.HandleCallbackAsync(data);

    //    if (success)
    //    {
    //        var orderId = Guid.Parse(data["orderId"]);

    //        var order = await _context.Set<Order>().FindAsync(orderId);
    //        order.MarkPaymentSuccess(data["transactionId"]);
    //        await _context.SaveChangesAsync();
    //    }

    //    return success;
    //}
}
