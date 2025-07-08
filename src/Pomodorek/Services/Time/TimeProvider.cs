namespace Pomodorek.Services;

public class TimeProvider : ITimeProvider
{
    public DateTimeOffset UtcNow
        => DateTimeOffset.UtcNow;

    public DateTimeOffset UnixEpoch
        => new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
}