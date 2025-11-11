using Application.Common.Interfaces;
using Application.Common.Utilities;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Models.Auth;
using Shared.Models.User;
using Shared.Web;
using System.Security.Claims;

namespace Infrastructure.Identity.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    AppIdentityDbContext appIdentityDbContext,
    IUnitOfWork unitOfWork,
    ICookieService cookieService,
    ITokenService tokenService,
    AppSettings appSettings,
    ICurrentUserProdvider currentUserProvider) : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly AppIdentityDbContext _appIdentityDbContext = appIdentityDbContext;
    private readonly ICookieService _cookieService = cookieService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly AppSettings _appSettings = appSettings;
    private readonly ICurrentUserProdvider _currentUserProvider = currentUserProvider;

    public async Task<UserReadModel> GetProfile(CancellationToken cancellationToken)
    {
        var userId = _currentUserProvider.GetCurrentUserId();
        var user = await _userManager.Users
            .Select(u => new UserReadModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
                AvatarUrl = u.AvatarUrl
            })
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new Exception("not found user");

        return user;
    }

    public async Task<TokenResult> SignIn(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
            ?? throw new Exception("Not found user");

        var checkpass = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (!checkpass.Succeeded)
        {
            throw new Exception("invalid check pass");
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopes = userClaims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(' ') ?? Array.Empty<string>();

        var token = await _tokenService.GenerateToken(request.Username, scopes, cancellationToken);

        _cookieService.Delete();
        _cookieService.Set(token.Token);

        return token;
    }

    public async Task SignOut()
    {
        _cookieService.Delete();
        await _signInManager.SignOutAsync();
    }

    public async Task SignUp(SignUpRequest request, CancellationToken cancellationToken)
    {
        if(await _userManager.FindByNameAsync(request.UserName) != null)
        {
            throw new Exception("exist username");
        }

        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            throw new Exception("exist email");
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var signup = await _userManager.CreateAsync(user, request.Password);

        if (!signup.Succeeded)
        {
            throw new Exception("occur error");
        }

        await _userManager.AddToRoleAsync(user, IdentityConstant.Role.User.ToString());

        string readScope = _appSettings.Identity.ScopeBaseDomain + "/read";
        string writeScope = _appSettings.Identity.ScopeBaseDomain + "/write";

        string[] scopes = [readScope, writeScope];

        var scopeClaim = new Claim("scope", string.Join(" ", scopes));

        await _userManager.AddClaimAsync(user, scopeClaim);
    }

    public async Task<TokenResult> RefreshToken(string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Id == _currentUserProvider.GetCurrentUserId());

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            throw new Exception("Token was expire");
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopeClaim = userClaims.FirstOrDefault(c => c.Type == "scope");

        var scopes = scopeClaim?.Value.Split(' ') ?? [];

        var result = await _tokenService.GenerateToken(user.UserName, scopes, cancellationToken);

        _cookieService.Delete();
        _cookieService.Set(result.Token);

        return new TokenResult();
    }

    public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new Exception("email not exist");

        var forgotPassword = await _appIdentityDbContext.ForgetPassword.FirstOrDefaultAsync(x => x.UserId == user.Id && x.OTP == request.OTP);

        var expireTime = forgotPassword.DateTime.AddMinutes(3);

        if (expireTime < DateTime.Now)
        {
            throw new Exception("OTP was expire");
        }

        var result = await _userManager.ResetPasswordAsync(user, forgotPassword.Token, request.NewPassword);

        if (!result.Succeeded)
            throw new Exception("Error reset password");
    }

    public async Task SendResetPassword(SendResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
           ?? throw new Exception("email not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var otp = StringHelper.Generate(10000, 99999);

        // save to db
        var forgotPassword = new ForgetPassword
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token,
            OTP = otp.ToString(),
            DateTime = DateTime.Now
        };

        await _appIdentityDbContext.AddAsync(forgotPassword, cancellationToken);
        // send email => hangfire
        //
        //
    }

}
