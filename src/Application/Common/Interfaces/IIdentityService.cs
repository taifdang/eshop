using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<RegisterUserResult> Register(RegisterUserRequest request);
    Task<TokenResult> Authenticate(LoginRequest request);
    Task Logout();
    Task<TokenResult> RefreshToken();
    Task<GetProfileResult> GetProfile();
}
