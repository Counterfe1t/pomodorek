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
    /// <param name="isStoppedManually">Boolean value indicating if the timer was stopped manually.</param>
    void Stop(bool isStoppedManually = false);
}