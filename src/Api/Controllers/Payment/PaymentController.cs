using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Payment;

[Route(BaseApiPath + "/payment")]
public class PaymentController : BaseController
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    //[HttpPost("create")]
    //public async Task<IActionResult> CreatePayment(CreatePaymentRequest req)
    //{
    //    var url = await _paymentService.CreatePaymentAsync(req);
    //    return Ok(new { url });
    //}


    //[HttpGet("callback/{provider}")]
    //public async Task<IActionResult> Callback(PaymentProvider provider)
    //{
    //    var data = Requested.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
    //    var ok = await _paymentService.HandleCallbackAsync(provider, data);
    //    return ok ? Ok("success") : BadRequest("fail");
    //}
}