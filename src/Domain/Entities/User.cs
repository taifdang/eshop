using Domain.SeedWork;

namespace Domain.Entities;

public class User : Entity<Guid>
{
    public string FullName { get; set; } = default!;
}
