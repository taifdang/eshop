using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Migrator;
using Shared.Constants;

namespace IdentityService.Infrastructure.Seed;

public class DataSeeder : IDataSeeder<IdentityContext>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IEventPublisher _eventPublisher;
    public DataSeeder(
        RoleManager<Role> roleManager,
        UserManager<User> userManager,
        IEventPublisher eventPublisher)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _eventPublisher = eventPublisher;
    }
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRoles();
        await SeedUsers();
    }

    public async Task SeedRoles()
    {
        if (!await _roleManager.Roles.AnyAsync())
        {
            if (!await _roleManager.RoleExistsAsync(IdentityConstant.Role.Admin))
            {
                await _roleManager.CreateAsync(new Role { Name = IdentityConstant.Role.Admin });
            }

            if (!await _roleManager.RoleExistsAsync(IdentityConstant.Role.User))
            {
                await _roleManager.CreateAsync(new Role { Name = IdentityConstant.Role.User });
            }
        }
    }

    public async Task SeedUsers()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            if (await _userManager.FindByNameAsync("peter") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.First(), "admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.First(), IdentityConstant.Role.Admin);
                    await _eventPublisher.PublishAsync(new UserCreatedIntegrationEvent
                    {
                        UserId = InitialData.Users.First().Id,
                        Email = InitialData.Users.First().Email!
                    });
                }
            }

            if (await _userManager.FindByNameAsync("mira") == null)
            {
                var result = await _userManager.CreateAsync(InitialData.Users.Last(), "user@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(InitialData.Users.Last(), IdentityConstant.Role.User);
                    await _eventPublisher.PublishAsync(new UserCreatedIntegrationEvent
                    {
                        UserId = InitialData.Users.Last().Id,
                        Email = InitialData.Users.Last().Email!
                    });
                }
            }
        }
    }
}