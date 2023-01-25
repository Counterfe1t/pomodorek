using Plugin.LocalNotification;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    public async Task DisplayNotification(string message) =>
        await LocalNotificationCenter.Current.Show(
            new NotificationRequest
            {
                Title = message,
                Subtitle = message,
                Description = message,
                CategoryType = NotificationCategoryType.Alarm,
            });
}
