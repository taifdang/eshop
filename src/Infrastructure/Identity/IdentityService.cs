using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;

namespace Infrastructure.Identity;

//ref: https://dotnettutorials.net/course/asp-net-core-identity-tutorials/
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ITokenService tokenService;
    private readonly ICookieService cookieService;
    private readonly AppIdentityDbContext dbContext;
    private readonly ICurrentUserProvider currentUserProvider;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        ICookieService cookieService,
        AppIdentityDbContext dbContext,
        ICurrentUserProvider currentUserProvider)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
        this.cookieService = cookieService;
        this.dbContext = dbContext;
        this.currentUserProvider = currentUserProvider;
    }
    public async Task<RegisterUserResult> Register(RegisterUserRequest request)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password
        };

        var identityResult = await userManager.CreateAsync(applicationUser, request.Password);
        var roleResult = await userManager.AddToRoleAsync(applicationUser, IdentityConstant.Role.User);

        if(identityResult.Succeeded == false)
        {
            throw new Exception(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }

        if(roleResult.Succeeded == false)
        {
            throw new Exception(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }

        return new RegisterUserResult(applicationUser.Id, applicationUser.UserName, applicationUser.Email);
    }

    public async Task<TokenResult> Authenticate(LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        Guard.Against.NotFound(request.UserName, user);
        
        var isValid = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (!isValid.Succeeded)
        {
            throw new Exception("Invalid check pass");
        }

        var roles = await userManager.GetRolesAsync(user);

        var result = tokenService.GenerateToken(user.Id, user.UserName!, user.Email!, roles.ToArray());
        var refreshToken = tokenService.GenerateRefereshToken();

        //save refresh token: expires 30 days
        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(30),
            Created = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();

        cookieService.Delete();
        cookieService.Set(refreshToken);

        return result;
    }

    public async Task Logout()
    {         
        var currentUser = currentUserProvider.GetCurrentUserId();
        if (!Guid.TryParse(currentUser, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID");
        }

        var refreshToken = cookieService.Get();

        if (!string.IsNullOrEmpty(refreshToken))
        {
            // revoke refresh token
            var IsToken = await dbContext.RefreshTokens
                .Where(x => x.Token == refreshToken && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (IsToken != null && IsToken.IsActive)
            {
                IsToken.Revoked = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();
            }

            cookieService.Delete();
        }

        await signInManager.SignOutAsync();
    }

    public async Task<TokenResult> RefreshToken()
    {
        //var currentUser = currentUserProvider.GetCurrentUserId();
        //if (!Guid.TryParse(currentUser, out var userId))
        //{
        //    throw new UnauthorizedAccessException("Invalid user ID");
        //}

        var userId = Guid.Empty;

        var tokenInCookie = cookieService.Get();

        if (!string.IsNullOrEmpty(tokenInCookie))
        {
            var lastToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenInCookie);

            if (lastToken != null && lastToken.IsActive)
            {
                lastToken.Revoked = DateTime.UtcNow;
                userId = lastToken.UserId;
            }  
        }

        if(userId == Guid.Empty)
        {
            throw new Exception("Not found user with token in cookie");
        }

        var user = await userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new Exception("Not found user");
        }

        var roles = await userManager.GetRolesAsync(user);

        var result = tokenService.GenerateToken(user.Id, user.UserName!, user.Email!, roles.ToArray());
        var refreshToken = tokenService.GenerateRefereshToken();

        //save refresh token
        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = result.Token,
            Expires = result.Expire,
            Created = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();

        cookieService.Delete();
        cookieService.Set(refreshToken);

        return result;
    }

    public async Task<GetProfileResult> GetProfile()
    {
        var currentUser = currentUserProvider.GetCurrentUserId();
        if(!Guid.TryParse(currentUser, out Guid userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID");
        }

        var user = await userManager.Users
            .Select(u => new GetProfileResult
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
                AvatarUrl = u.AvatarUrl
            })
            .SingleOrDefaultAsync(x => x.Id == userId)
            ?? throw new Exception("not found user");

        return user;
    }
}
