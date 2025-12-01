using Domain.Common;

namespace Domain.Entities;

public class OutBoxMessage : Entity<Guid>
{
    public string EventType { get; set; } 
    public string Payload { get; set; }
    public bool IsPublished { get; set; }
}
