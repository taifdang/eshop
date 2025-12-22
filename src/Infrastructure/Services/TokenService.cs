using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly AppSettings appSettings;

    public TokenService(AppSettings appSettings)
    {
        this.appSettings = appSettings;   
    }

    public TokenResult GenerateToken(Guid userId, string name, string email, string[] roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString())
        };

        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Identity.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireTime = DateTime.UtcNow.AddMinutes(appSettings.Identity.ExpiredTime);

        var token = new JwtSecurityToken(
            issuer: appSettings.Identity.Authority,
            audience: appSettings.Identity.Audience,
            claims: claims,
            expires: expireTime,
            signingCredentials: credentials);

        return new TokenResult
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expire = expireTime
        };
    }
    public string GenerateRefereshToken()
    {
        var bytes = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}
