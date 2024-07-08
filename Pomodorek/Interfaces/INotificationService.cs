namespace Pomodorek.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Display notification native to device operating system.
    /// </summary>
    /// <param name="notification"><see cref="NotificationModel" /> object containing notification properties.</param>
    Task DisplayNotificationAsync(NotificationModel notification);
}