using Android.App;
using AndroidX.Core.App;
using AndroidApp = Android.App.Application;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationManager _manager;

    public NotificationService()
    {
        _manager = NotificationManager.FromContext(AndroidApp.Context);
    }

    public void DisplayNotification(string message)
    {
        using var builder = new NotificationCompat.Builder(AndroidApp.Context, nameof(NotificationService));

        builder.SetContentTitle(Constants.ApplicationName);
        builder.SetContentText(message);
        builder.SetSmallIcon(Resource.Drawable.mtrl_ic_error);

        var notification = builder.Build();
        _manager.Notify(0, notification);
    }
}