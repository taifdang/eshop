using Application.Order.Commands.CreateOrder;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Order;

[Route(BaseApiPath + "/order")]
public class OrdersController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
