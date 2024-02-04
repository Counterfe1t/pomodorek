namespace Pomodorek.Services;

public interface ISessionService
{
    void StartInterval(SessionModel session);

    void FinishInterval(SessionModel session);

    int GetIntervalLengthInMin(IntervalEnum interval);

    int GetIntervalLengthInSec(IntervalEnum interval);
}