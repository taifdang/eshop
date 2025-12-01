using Application.Common.Interfaces.Services;
using Application.Customer.Commands;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Auth;
using Shared.Web;

namespace Api.Controllers.Identity;

[Route(BaseApiPath + "/auth")]
public class AuthController(IIdentityService identityService) : BaseController
{
    private readonly IIdentityService _identityService = identityService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(SignInRequest request, CancellationToken cancellationToken)
    {
        var tokenResult = await _identityService.Authenticate(request, cancellationToken);
        return Ok(tokenResult); 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(SignUpRequest request, CancellationToken cancellationToken)
    {
        var userId = await _identityService.Register(request, cancellationToken);

        await Mediator.Send(new CreateCustomerCommand(userId, request.Email));

        return NoContent();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _identityService.Logout();
        return NoContent();
    }
}
