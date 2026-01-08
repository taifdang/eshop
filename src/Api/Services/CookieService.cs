using Application.Common.Interfaces;

namespace Api.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Get()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"] ?? string.Empty;
    }
    // SameSite = SameSiteMode.None, Secure = true when cross-site
    public void Set(string token) => _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                SameSite = SameSiteMode.Lax, //SameSite
                Secure = false,
                HttpOnly = false,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
            });

    public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
}
