using Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Models.User;
using Shared.Web;
using System.Security.Claims;

namespace Api.Controllers.Identity;

[Route(BaseApiPath + "/user")]
public class UserController(IUserManagerService userManagerService) : BaseController
{
    private readonly IUserManagerService _userManagerService = userManagerService;

    [HttpGet]
    [Authorize(Roles = IdentityConstant.Role.Admin)]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        return Ok(await _userManagerService.GetList(cancellationToken));
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(AssignRoleRequest request , CancellationToken cancellationToken)
    {
        await _userManagerService.AssignRole(request, cancellationToken);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateRequest request, CancellationToken cancellationToken)
    {
        await _userManagerService.Update(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string userId)
    {
        await _userManagerService.Delete(userId);
        return NoContent();
    }
    [HttpGet("info")]
    public IActionResult info()
    {
        return Ok(new
        {
            user = User.Identity?.Name,
            roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
        });
    }

}
