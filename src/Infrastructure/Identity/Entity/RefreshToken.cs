namespace Infrastructure.Identity.Entity;

public class RefreshToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public ApplicationUser User { get; set; }
}
