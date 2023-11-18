namespace Pomodorek.Services;

public interface INotificationService
{
    Task DisplayNotificationAsync(Notification notification);
}