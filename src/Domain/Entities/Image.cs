namespace Domain.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string BaseUrl { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string AllText { get; set; } = default!;
}