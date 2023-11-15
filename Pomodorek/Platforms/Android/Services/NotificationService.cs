using Android.App;
using Android.OS;
using AndroidX.Core.App;
using AndroidApp = Android.App.Application;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    private const string _channelId = "General";

    private readonly NotificationManager _manager;

    public NotificationService()
    {
        _manager = NotificationManager.FromContext(AndroidApp.Context);
    }

    public async Task DisplayNotificationAsync(string message)
    {
        using var builder = new NotificationCompat.Builder(AndroidApp.Context, nameof(NotificationService));

        builder.SetContentTitle(Constants.ApplicationName);
        builder.SetContentText(message);
        builder.SetPriority((int)NotificationPriority.Max);

        // TODO: Add custom icon
        builder.SetSmallIcon(Resource.Drawable.ic_clock_black_24dp);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(_channelId, _channelId, NotificationImportance.Max);
            _manager.CreateNotificationChannel(channel);

            builder.SetChannelId(_channelId);
        }

        _manager.Notify(0, builder.Build());
    }
}