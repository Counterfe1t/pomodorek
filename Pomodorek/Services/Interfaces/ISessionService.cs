namespace Pomodorek.Interfaces;

public interface ISessionService
{
    Session GetNewSession();

    void StartInterval(Session session);

    void FinishInterval(Session session);

    int GetIntervalLengthInMin(IntervalEnum interval);
}