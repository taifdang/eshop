using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Entity;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual Role Role { get; set; }
}
