using Android.App;
using Android.OS;
using Android.Runtime;
using Pomodorek.Platforms.Android.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace Pomodorek;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        base.OnCreate();
        CreateNotificationChannel();
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var manager = NotificationManager.FromContext(Context);
            var channel = new NotificationChannel(
                NotificationHelper.NotificaionChannelId,
                NotificationHelper.NotificaionChannelId,
                NotificationImportance.Max);
            manager.CreateNotificationChannel(channel);
        }
    }
}