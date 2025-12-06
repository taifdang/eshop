using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Payments;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.ExternalServices;

public class PaymentService
{
    private readonly PaymentGatewayFactory _factory;
    private readonly ApplicationDbContext _context;

    public PaymentService(PaymentGatewayFactory factory, ApplicationDbContext db)
    {
        _factory = factory;
        _context = db;
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
