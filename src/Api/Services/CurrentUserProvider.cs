using Application.Abstractions;
using System.Security.Claims;

namespace Api.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _context;

    public CurrentUserProvider(IHttpContextAccessor context)
    {
        _context = context;
    }

    public string? GetCurrentUserId()
    {
        var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? _context.HttpContext.User.FindFirst("sub")?.Value;

        return userId;
    }
}