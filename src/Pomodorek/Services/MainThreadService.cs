namespace Pomodorek.Services;

public class MainThreadService : IMainThreadService
{
    public void BeginInvokeOnMainThread(Action action)
    {
        if (MainThread.IsMainThread)
            action.Invoke();
        else
            MainThread.BeginInvokeOnMainThread(() => action.Invoke());
    }
}