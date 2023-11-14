using Android.App;
using Android.OS;
using AndroidX.Core.App;
using Pomodorek.Platforms.Android;
using AndroidApp = Android.App.Application;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationManager _manager;

    public NotificationService()
    {
        _manager = NotificationManager.FromContext(AndroidApp.Context);
    }

    public async Task DisplayNotificationAsync(string message)
    {
        if (await RequestNotificationPermissionAsync() == false)
        {
            // TODO: Log error
            return;
        }

        using var builder = new NotificationCompat.Builder(AndroidApp.Context, nameof(NotificationService));

        builder.SetContentTitle(Constants.ApplicationName);
        builder.SetContentText(message);
        builder.SetPriority((int)NotificationPriority.Max);

        // TODO: Add custom icon
        builder.SetSmallIcon(Resource.Drawable.abc_seekbar_tick_mark_material);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelId = "general";
            var channel = new NotificationChannel(channelId, channelId, NotificationImportance.Max);

            _manager.CreateNotificationChannel(channel);
            builder.SetChannelId(channelId);
        }

        var notification = builder.Build();
        _manager.Notify(0, notification);
    }

    private static async Task<bool> RequestNotificationPermissionAsync()
    {
        if (await Permissions.CheckStatusAsync<NotificationPermission>() == PermissionStatus.Granted)
            return true;

        return await Permissions.RequestAsync<NotificationPermission>() == PermissionStatus.Granted;
    }
}