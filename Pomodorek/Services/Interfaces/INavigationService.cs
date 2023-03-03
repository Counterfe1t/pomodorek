namespace Pomodorek.Services;

public interface INavigationService
{
    Task GoToSettingsPageAsync();
    Task GoToTimerPageAsync();
}
