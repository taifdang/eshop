using Domain.SeedWork;

namespace Domain.Entities;

public class Customer : Entity<Guid>
{
    //public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; } 
    public string? Address { get; set; }
}