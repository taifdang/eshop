using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    TokenResult GenerateToken(Guid userId, string name, string email, string[] roles);
    string GenerateRefereshToken();
}
