using Application.Order.Commands.CreateOrder;
using Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Order;

[Route(BaseApiPath + "/order")]
public class OrdersController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrder([FromForm] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request.CustomerId, request.Street, request.City, request.ZipCode);

        return Ok(await Mediator.Send(command));
    }
}
