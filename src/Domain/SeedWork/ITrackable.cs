namespace Domain.SeedWork;

public interface ITrackable
{
    long Version { get; set; }
    DateTime? CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}
