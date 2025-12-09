using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entity;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationRole Role { get; set; }
}
