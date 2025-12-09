using Application.Common.Interfaces;
using Infrastructure.Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity.Services;

public class TokenService(
    AppSettings appSettings, 
    UserManager<ApplicationUser> userManager) 
    : ITokenService
{
    private readonly AppSettings _appSettings = appSettings;
    private readonly Shared.Constants.Identity _jwt = appSettings.Identity;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<TokenResult> GenerateToken(string username, string[] scopes, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(username);

        var roles = await _userManager.GetRolesAsync(user);

        var result = new TokenResult();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var expires = DateTime.UtcNow.AddDays(_jwt.ExpiredTime);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                audience: _appSettings.Identity.Audience,
                issuer: _appSettings.Identity.Authority,
                signingCredentials: credentials
            );
        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        result.Token = tokenResult;
        result.Expire = expires;

        return result;

        //token
        //var user = await _userManager.FindByNameAsync(username);
        //if (user == null) throw new EntityNotFoundException(username);

        //var roles = await _userManager.GetRolesAsync(user);


        //var claims = new[]
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim(ClaimTypes.Email, user.Email),
        //    new Claim(ClaimTypes.Uri, user?.AvatarUrl ?? "default.png"),
        //    new Claim(ClaimTypes.Role, roles == null ? IdentityConstant.Role.User.ToString() : string.Join(";", roles)),
        //    new Claim("scope", string.Join(" ", scopes)) // Adding scope claim
        //};

        //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
        //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //var expires = DateTime.UtcNow.AddDays(_jwt.ExpiredTime);

        //var token = new JwtSecurityToken(
        //    issuer: "https://localhost:7129",
        //    audience: "ecommerce-monolith",
        //    claims: claims,
        //    expires: expires,
        //    signingCredentials: credentials);

        //var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        ////set result
        //result.Token = tokenResult;
        //result.UserId = user.Id;
        //result.Expire = expires;

        ////refresh token  
        //var refreshToken = new RefreshToken
        //{
        //    Token = tokenResult,
        //    UserId = user.Id,
        //    Expires = expires,
        //    Created = DateTime.UtcNow
        //};

        //var existToken = await _appIdentityDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);

        //if (existToken == null)
        //{
        //    await _unitOfWork.ExecuteTransactionAsync(async() =>
        //         await _appIdentityDbContext.RefreshTokens.AddAsync(refreshToken), cancellationToken);
        //}

        //else if (existToken.Expires > DateTime.UtcNow)
        //{
        //    existToken.Token = tokenResult;
        //    existToken.Expires = expires;
        //    existToken.Created = DateTime.UtcNow;

        //    await _appIdentityDbContext.SaveChangesAsync(cancellationToken);
        //}
        //else
        //{
        //    await _unitOfWork.ExecuteTransactionAsync(
        //       async () =>
        //       {
        //           _appIdentityDbContext.RefreshTokens.Remove(existToken);
        //           await _appIdentityDbContext.RefreshTokens.AddAsync(refreshToken);
        //       }, cancellationToken);
        //}
        //return result;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        TokenValidationParameters validationParameters = new()
        {
            ValidIssuer = _appSettings.Identity.Authority,
            ValidAudience = _appSettings.Identity.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

        return principal;
    }
}
