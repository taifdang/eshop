using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.EFCore;

namespace Infrastructure.Identity.Data.Seed;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public IdentityDataSeeder(
        RoleManager<ApplicationRole> roleManager,
        AppIdentityDbContext appIdentityDbContext,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _appIdentityDbContext = appIdentityDbContext;
        _userManager = userManager;
    }

    public async Task SendAllAsync()
    {
        var pendingMigrations = await _appIdentityDbContext.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
        {
            await SeedRoles();
            await SeedUsers();
        }
    }

    public async Task SeedRoles()
    {
        if(!await _roleManager.Roles.AnyAsync())
        {
            if(!await _roleManager.RoleExistsAsync(IdentityConstant.Role.Admin))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = IdentityConstant.Role.Admin });
            }

            if (!await _roleManager.RoleExistsAsync(IdentityConstant.Role.User))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = IdentityConstant.Role.User });
            }
        }
    }

    public async Task SeedUsers()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            if(await _userManager.FindByNameAsync("peter") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.First(), "admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.First(), IdentityConstant.Role.Admin);
                }
            }

            if (await _userManager.FindByNameAsync("mira") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.Last(), "user@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.Last(), IdentityConstant.Role.User);
                }
            }
        }
    }
}
