using CommunityToolkit.Maui.Alerts;

namespace Pomodorek.Services;

public class NotificationService : INotificationService
{
    public async Task DisplayNotificationAsync(Notification notification)
    {
        await Toast.Make(notification.Content).Show();

        // TODO: Customize windows notifications
        //var builder = new AppNotificationBuilder()
        //    .AddText(notification.Title)
        //    .AddText(notification.Content)
        //    .SetTimeStamp(notification.TriggerAlarmAt)
        //    // TODO: Customize windows notification
        //    .SetScenario(AppNotificationScenario.Alarm)
        //    .AddButton(new AppNotificationButton("Dismiss")
        //        .AddArgument("action", "dismiss"));
        //builder.BuildNotification();
    }
}