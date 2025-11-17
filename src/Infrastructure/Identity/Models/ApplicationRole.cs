using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}
