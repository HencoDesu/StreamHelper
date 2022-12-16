namespace StreamHelper.Core.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan Seconds(this int seconds)
        => TimeSpan.FromSeconds(seconds);
    
    public static TimeSpan Minutes(this int minutes)
        => TimeSpan.FromMinutes(minutes);
}