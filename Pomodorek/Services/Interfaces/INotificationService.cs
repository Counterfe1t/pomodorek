namespace Pomodorek.Services;

public interface INotificationService
{
    Task DisplayNotificationAsync(string message);
}