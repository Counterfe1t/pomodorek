namespace Pomodorek.Services;

public interface ISessionService
{
    /// <summary>
    /// Get details of the session currently in progress.
    /// </summary>
    /// <returns><see cref="SessionModel" /> object containing session data.</returns>
    SessionModel GetSession();

    /// <summary>
    /// Save session data.
    /// </summary>
    /// <param name="session"><see cref="SessionModel" /> object containing session data.</param>
    void SaveSession(SessionModel session);

    /// <summary>
    /// Start the current interval.
    /// </summary>
    /// <param name="session"><see cref="SessionModel" /> object containing session data.</param>
    void StartInterval(SessionModel session);

    /// <summary>
    /// Finish the current interval.
    /// </summary>
    /// <param name="session"><see cref="SessionModel" /> object containing session data.</param>
    void FinishInterval(SessionModel session);

    /// <summary>
    /// Get interval length value in seconds.
    /// </summary>
    /// <param name="interval"><see cref="IntervalEnum" /> enum representing interval type.</param>
    /// <returns>Integer value representing interval length in seconds.</returns>
    int GetIntervalLengthInSec(IntervalEnum interval);
}