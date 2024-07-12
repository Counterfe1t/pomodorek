using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using Pomodorek.Platforms.Windows.Helpers;

namespace Pomodorek.WinUI;

public class Program
{
    public static App App { get; set; }

    public static MauiWinUIWindow CurrentWindow { get; set; }

    [STAThread]
    private static async Task<int> Main()
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();

        var isRedirect = await DecideRedirection();
        if (!isRedirect)
        {
            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                App = new App();
            });
        }

        return 0;
    }

    private static async Task<bool> DecideRedirection()
    {
        var isRedirect = false;
        var args = AppInstance.GetCurrent().GetActivatedEventArgs();
        var keyInstance = AppInstance.FindOrRegisterForKey(string.Empty);

        if (keyInstance.IsCurrent)
        {
            keyInstance.Activated += OnActivated;
            return isRedirect;
        }

        isRedirect = true;
        await keyInstance.RedirectActivationToAsync(args);

        return isRedirect;
    }

    private static void OnActivated(object sender, AppActivationArguments args)
    {
        if (App is null)
        {
            throw new Exception("App should not be null");
        }

        WindowHelper.ActivateWindow(CurrentWindow);
    }
}