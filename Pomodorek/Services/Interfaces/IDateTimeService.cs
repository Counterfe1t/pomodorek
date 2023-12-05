namespace Pomodorek.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }

    DateTime UnixEpoch { get; }
}