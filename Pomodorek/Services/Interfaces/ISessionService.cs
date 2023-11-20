namespace Pomodorek.Services;

public interface ISessionService
{
    void StartInterval(Session session);

    void FinishInterval(Session session);

    int GetIntervalLengthInMin(IntervalEnum interval);

    int GetIntervalLengthInSec(IntervalEnum interval);
}