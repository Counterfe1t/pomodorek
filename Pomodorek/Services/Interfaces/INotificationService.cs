namespace Pomodorek.Services;

public interface INotificationService
{
    Task DisplayNotificationAsync(NotificationModel notification);
}