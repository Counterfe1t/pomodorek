using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
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