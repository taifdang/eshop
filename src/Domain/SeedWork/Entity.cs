namespace Domain.SeedWork;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long Version { get; set; }
}