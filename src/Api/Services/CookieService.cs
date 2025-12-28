using Application.Common.Interfaces;

namespace Api.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Get()
    {
        //var token = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        //return string.IsNullOrEmpty(token)
        //    ? throw new Exception("not exist token")
        //    : token;
        return _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"] ?? string.Empty;
    }


    public void Set(string token) => _httpContextAccessor.HttpContext?.Response.Cookies.Append(
            "refreshToken",
            token,
            new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                MaxAge = TimeSpan.FromDays(30)
            });

    public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
}
