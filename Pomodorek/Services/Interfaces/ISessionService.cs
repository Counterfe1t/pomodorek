namespace Pomodorek.Services;

public interface ISessionService
{
    SessionModel GetSession();

    void SaveSession(SessionModel session);

    void StartInterval(SessionModel session);

    void FinishInterval(SessionModel session);

    int GetIntervalLengthInMin(IntervalEnum interval);

    int GetIntervalLengthInSec(IntervalEnum interval);
}