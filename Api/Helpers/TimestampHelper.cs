namespace Api.Helpers;

internal static class TimestampHelper
{
    public static long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}