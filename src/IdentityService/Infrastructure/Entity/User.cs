using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Entity;

public class User : IdentityUser<Guid>
{
    public IList<UserRole> UserRoles { get; set; }
    public IList<PasswordHistory> PasswordHistories { get; set; }
}
