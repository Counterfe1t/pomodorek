namespace Pomodorek.Interfaces;

public interface IMainThreadService
{
    /// <summary>
    /// Invoke action on the main thread if necessary.
    /// </summary>
    /// <param name="action">Action to be invoked.</param>
    void BeginInvokeOnMainThread(Action action);
}