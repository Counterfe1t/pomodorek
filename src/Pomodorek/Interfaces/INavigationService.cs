namespace Pomodorek.Interfaces;

public interface INavigationService
{
    /// <summary>
    /// Navigate to the timer page.
    /// </summary>
    Task NavigateToTimerPageAsync();

    /// <summary>
    /// Navigate to the settings page.
    /// </summary>
    Task NavigateToSettingsPageAsync();
}