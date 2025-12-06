namespace TransactionalOutbox.Abstractions;

public class PollingOutboxMessage
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public string Payload { get; set; } = default!;
    public string PayloadType { get; set; } = default!;
    public DateTime? ProcessedDate { get; set; }
    public int RetryCount { get; set; }
}
