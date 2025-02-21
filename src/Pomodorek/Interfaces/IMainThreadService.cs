namespace Pomodorek.Interfaces;

public interface IMainThreadService
{
    /// <summary>
    /// Invoke <see cref="Action"/> on the main thread if necessary.
    /// </summary>
    /// <param name="action"><see cref="Action"/> to be invoked.</param>
    void BeginInvokeOnMainThread(Action action);
}