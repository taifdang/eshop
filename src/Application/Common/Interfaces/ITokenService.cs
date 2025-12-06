using Shared.Models.Auth;
using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    //Task<TokenResult> GenerateToken(ApplicationUserModel user, string[] scopes, CancellationToken cancellationToken);
    Task<TokenResult> GenerateToken(string username, string[] scopes, CancellationToken cancellationToken);
    ClaimsPrincipal ValidateToken(string token);
}
