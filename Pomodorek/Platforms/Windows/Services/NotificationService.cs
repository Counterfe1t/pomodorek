using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    // TODO: Clicking on the notification starts new application instance.
    // Follow example on https://github.com/dotnet/maui/issues/9973 to possibly fix this issue.
    public async Task DisplayNotificationAsync(NotificationModel notification) =>
        await Task.Run(() =>
        {
            var toast = new AppNotificationBuilder()
            .AddText(notification.Title)
            .AddText(notification.Content)
            .SetScenario(AppNotificationScenario.Default)
            .MuteAudio()
            .BuildNotification();

            AppNotificationManager.Default.Show(toast);
        });
}