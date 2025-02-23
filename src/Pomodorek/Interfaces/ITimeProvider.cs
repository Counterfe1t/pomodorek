namespace Pomodorek.Interfaces;

public interface ITimeProvider
{
    /// <summary>
    /// Get current date and time in UTC format.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Get number of non-leap seconds that have elapsed since 01.01.1970 00:00:00 UTC.
    /// </summary>
    DateTime UnixEpoch { get; }
}