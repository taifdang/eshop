using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? AvatarUrl { get; set; }

    //public Customer? Customers { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}
