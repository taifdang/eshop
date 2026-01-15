namespace Application.Common.Utilities;

public static class TimeHepler
{
    public static long GetCurrentTimeTicks()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
