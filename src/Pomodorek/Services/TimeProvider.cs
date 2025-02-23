namespace Pomodorek.Services;

public class TimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime UnixEpoch => new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}