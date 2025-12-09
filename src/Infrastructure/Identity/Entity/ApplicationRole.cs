using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entity;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}
