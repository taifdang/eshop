using Application.Basket.Commands.UpdateItem;
using Application.Basket.Queries.GetCartList;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Basket;

[Route(BaseApiPath + "/basket")]
public class BasketController(ICurrentUserProdvider currentUserProdvider) : BaseController
{
    public readonly ICurrentUserProdvider _currentUserProdvider = currentUserProdvider;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBasket()
    {
        return Ok(await Mediator.Send(new GetBasketQuery()));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromBody]UpdateItemCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
