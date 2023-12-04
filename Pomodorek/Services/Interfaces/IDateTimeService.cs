namespace Pomodorek.Services;

public interface IDateTimeService
{
    DateTime Now { get; }

    DateTime UnixEpoch { get; }
}