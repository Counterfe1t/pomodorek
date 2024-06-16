namespace Pomodorek.Services;

public interface ITimerService
{
    /// <summary>
    /// Start the timer.
    /// </summary>
    /// <param name="callback"><see cref="Action" /> to be invoked on every tick.</param>
    void Start(Action callback);

    /// <summary>
    /// Stop the timer.
    /// </summary>
    /// <param name="isCancelled">Boolean value indicating if the timer was cancelled manually.</param>
    void Stop(bool isCancelled = false);
}