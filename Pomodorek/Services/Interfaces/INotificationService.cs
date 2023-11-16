namespace Pomodorek.Services;

public interface INotificationService
{
    Task DisplayNotificationAsync(NotificationDto notification);
}