namespace Application.Common.Models;

public class IntegrationEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}