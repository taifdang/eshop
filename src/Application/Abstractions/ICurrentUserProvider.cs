namespace Application.Abstractions;

public interface ICurrentUserProvider
{
    string? GetCurrentUserId();
}
