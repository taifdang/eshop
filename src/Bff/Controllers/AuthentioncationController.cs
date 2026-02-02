using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Bff.Controllers;

[ApiController]
[Route("bff")]
public class AuthenticationController : Controller
{
    private readonly IAntiforgery _forgeryService;

    public AuthenticationController(IAntiforgery forgeryService)
    {
        _forgeryService = forgeryService;
    }

    [HttpGet("login")]
    public async Task LoginAsync(string returnUrl)
    {
        var targetReturnUrl = RedirectToSpa(returnUrl);

        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            Response.Redirect(targetReturnUrl);
            return; 
        }
        else
        {
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = targetReturnUrl
            });
        }
    }

    [HttpGet("logout")]
    public async Task LogoutAsync(string? returnUrl = null)
    {
        // Determine target return URL (default to SPA root)
        var targetReturnUrl = string.IsNullOrEmpty(returnUrl) || !returnUrl.StartsWith("http://localhost:3000")
            ? "http://localhost:3000/"
            : returnUrl;

        // Sign out from local cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        // Sign out from OpenID Connect and redirect to SPA
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = targetReturnUrl
        });
    }

    [HttpGet("userinfo")]
    public IActionResult GetUserInfo()
    {
        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            var tokens = _forgeryService.GetAndStoreTokens(HttpContext);
            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions { HttpOnly = false });

            return Ok(new
            {
                UserId = HttpContext.User.FindFirst("sub")?.Value,
                UserName = HttpContext.User.FindFirst("name")?.Value,
                Email = HttpContext.User.FindFirst("email")?.Value,
                Roles = HttpContext.User.FindAll("role").Select(c => c.Value).ToArray(), // Include roles
                Claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        return Unauthorized();
    }

    private string RedirectToSpa(string returnUrl)
    {
        var spaUrl = "http://localhost:3000/";

        var targetReturnUrl = !string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith(spaUrl)
           ? returnUrl
           : spaUrl;

        return targetReturnUrl;
    }
}
