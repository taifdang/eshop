using Application.Customer.Queries.GetCustomerByUserId;
using Microsoft.AspNetCore.Mvc;
using Shared.Web;

namespace Api.Controllers.Customers;

[Route(BaseApiPath + "/customer")]
public class CustomersController(ICurrentUserProdvider currentUserProdvider) : BaseController
{
    private readonly ICurrentUserProdvider _currentUserProdvider = currentUserProdvider;

    [HttpGet]
    public async Task<IActionResult> GetCurrentCustomer()
    {
        var userId = _currentUserProdvider.GetCurrentUserId();

        if(userId == null)
            return Unauthorized();

        return Ok(await Mediator.Send(new GetCustomerByUserIdQuery(Guid.Parse(userId))));
    }
}
