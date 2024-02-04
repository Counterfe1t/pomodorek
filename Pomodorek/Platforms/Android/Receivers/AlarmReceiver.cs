using Android.Content;
using Pomodorek.Platforms.Android.Services;

namespace Pomodorek.Platforms.Android.Receivers;

[BroadcastReceiver(Enabled = true)]
public class AlarmReceiver : BroadcastReceiver
{
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;

    public AlarmReceiver()
    {
        _notificationService = ServiceHelper.GetService<INotificationService>();
        _settingsService = ServiceHelper.GetService<ISettingsService>();
    }

    public override async void OnReceive(Context context, Intent intent)
    {
        if (MainThread.IsMainThread)
            await DisplayNotificationAsync();
        else
            MainThread.BeginInvokeOnMainThread(async () => await DisplayNotificationAsync());
    }

    private async Task DisplayNotificationAsync()
    {
        var serializedNotification = _settingsService.Get(nameof(NotificationModel), string.Empty);

        if (string.IsNullOrWhiteSpace(serializedNotification))
        {
            // TODO: Add error logging
            return;
        }

        var notification = JsonSerializer.Deserialize<NotificationModel>(serializedNotification);
        notification.Id = 1337;
        notification.CurrentProgress = 0;
        notification.MaxProgress = 0;
        notification.IsOngoing = false;
        notification.OnlyAlertOnce = false;

        await _notificationService.DisplayNotificationAsync(notification);
    }
}