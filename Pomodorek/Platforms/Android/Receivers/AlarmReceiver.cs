using Android.Content;

namespace Pomodorek.Platforms.Android.Receivers;

[BroadcastReceiver(Enabled = true)]
public class AlarmReceiver : BroadcastReceiver
{
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;

    public AlarmReceiver()
    {
        _notificationService = (INotificationService)MauiApplication.Current.Services.GetService(typeof(INotificationService));
        _settingsService = (ISettingsService)MauiApplication.Current.Services.GetService(typeof(ISettingsService));
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
        var serializedNotification = _settingsService.Get(nameof(Notification), string.Empty);
        var notification = JsonSerializer.Deserialize<Notification>(serializedNotification);
        notification.Id = 1337;
        notification.CurrentProgress = 0;
        notification.MaxProgress = 0;
        notification.IsOngoing = false;
        notification.OnlyAlertOnce = false;

        await _notificationService.DisplayNotificationAsync(notification);
    }
}