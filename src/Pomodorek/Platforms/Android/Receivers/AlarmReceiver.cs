using Android.Content;

namespace Pomodorek.Platforms.Android.Receivers;

[BroadcastReceiver(Enabled = true)]
public class AlarmReceiver : BroadcastReceiver
{
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IMainThreadService _mainThreadService;

    public AlarmReceiver()
    {
        _notificationService = Services.ServiceProvider.GetService<INotificationService>();
        _settingsService = Services.ServiceProvider.GetService<ISettingsService>();
        _mainThreadService = Services.ServiceProvider.GetService<IMainThreadService>();
    }

    public override void OnReceive(Context context, Intent intent)
        => _mainThreadService.BeginInvokeOnMainThread(async () => await DisplayNotificationAsync());

    private async Task DisplayNotificationAsync()
    {
        var serializedNotification = _settingsService.Get(nameof(NotificationModel), string.Empty);

        if (string.IsNullOrWhiteSpace(serializedNotification))
        {
            // TODO Add error logging
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