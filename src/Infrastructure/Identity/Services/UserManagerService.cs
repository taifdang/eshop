using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models.User;
using System.Security.Claims;

namespace Infrastructure.Identity.Services;

public class UserManagerService : IUserManagerService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _storageService;

    public UserManagerService(UserManager<ApplicationUser> userManager, IFileService storageService)
    {
        _userManager = userManager;
        _storageService = storageService;
    }
    public async Task<List<UserReadModel>> GetList(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Select(x => new UserReadModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Roles = x.UserRoles.Select(x => x.Role.Name).ToList(),
                AvatarUrl = x.AvatarUrl,
                Status = x.LockoutEnd != null && x.LockoutEnd > DateTimeOffset.Now ? "Locked" : "Active"
            })
            .ToListAsync(cancellationToken);

        return users;
    }
    public async Task AssignRole(AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new Exception("Not find user");

        // 1. Roles
        // format role: list role choosen (checkbox)
        var currentRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();

        // except to get difference between 2 list, example:
        // currentRoles = [Admin, User], selectedRoles = [User, Manager]
        // rolesToAdd = [Manager], rolesToRemove = [Admin]

        var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(selectedRoles).ToList();

        if (rolesToRemove.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        if (rolesToAdd.Any())
        {
            await _userManager.AddToRolesAsync(user, rolesToAdd);
        }

        // 2. Scopes (similar to roles)
        var currentClaims = await _userManager.GetClaimsAsync(user);
        var selectedScopes = request.Scopes.Where(x => x.Selected).Select(x => x.Name).ToList();

        var scopeClaim = currentClaims.FirstOrDefault(c => c.Type == "scope");

        if (scopeClaim != null)
        {
            await _userManager.RemoveClaimAsync(user, scopeClaim);
        }
        if (selectedScopes.Any())
        {
            var newScopeClaim = new Claim("scope", string.Join(' ', selectedScopes));
            await _userManager.AddClaimAsync(user, newScopeClaim);
        }
    }
    public async Task Update(UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new Exception("Not find user");

        user.AvatarUrl = request.Avatar;

        // update file & save image
        if (request.MediaFile != null)
        {
            if (user.AvatarUrl != null)
            {
                await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = user.AvatarUrl });
            }
            await _storageService.AddFileAsync(request.MediaFile);
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception("Update user failed");
        }
    }
    public async Task Delete(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new Exception("Not find user");

        if (user.AvatarUrl != null)
        {
            await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = user.AvatarUrl });
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception("Delete user failed");
        }
    }
}
