namespace Pomodorek.Services;

public interface IDateTimeService
{
    /// <summary>
    /// Current date and time in UTC format.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// The number of non-leap seconds that have elapsed since 01.01.1970 00:00:00 UTC.
    /// </summary>
    DateTime UnixEpoch { get; }
}