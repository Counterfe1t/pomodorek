using Android.Content;
using ServiceProvider = Pomodorek.Platforms.Android.Services.ServiceProvider;

namespace Pomodorek.Platforms.Android.Receivers;

[BroadcastReceiver(Enabled = true)]
public class AlarmReceiver : BroadcastReceiver
{
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;

    public AlarmReceiver()
    {
        _notificationService = ServiceProvider.GetService<INotificationService>();
        _settingsService = ServiceProvider.GetService<ISettingsService>();
    }

    public override async void OnReceive(Context? context, Intent? intent)
        => await MainThread.InvokeOnMainThreadAsync(DisplayNotificationAsync);

    private async Task DisplayNotificationAsync()
    {
        var serializedNotification = _settingsService.Get(nameof(NotificationModel), string.Empty);
        if (string.IsNullOrWhiteSpace(serializedNotification))
        {
            // TODO Add error logging
            return;
        }

        var notification = JsonSerializer.Deserialize<NotificationModel>(serializedNotification);
        if (notification is null)
        {
            // TODO Add error logging
            return;
        }

        notification.Id = 1337;
        notification.CurrentProgress = 0;
        notification.MaxProgress = 0;
        notification.IsOngoing = false;
        notification.OnlyAlertOnce = false;

        await _notificationService.DisplayNotificationAsync(notification);
    }
}