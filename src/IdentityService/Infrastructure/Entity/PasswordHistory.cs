namespace IdentityService.Infrastructure.Entity;

public class PasswordHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; }
    public virtual User User { get; set; }
}
