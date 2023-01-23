namespace Pomodorek.Services;

public interface INotificationService
{
    Task DisplayNotification(string message);
}
