namespace Pomodorek.Services;

public interface INavigationService
{
    /// <summary>
    /// Navigate to the settings page.
    /// </summary>
    Task GoToSettingsPageAsync();

    /// <summary>
    /// Navigate to the timer page.
    /// </summary>
    Task GoToTimerPageAsync();
}