namespace Pomodorek.Services;

public class NavigationService : INavigationService
{
    public async Task GoToTimerPageAsync() => await Shell.Current.GoToAsync("//TimerPage");

    public async Task GoToSettingsPageAsync() => await Shell.Current.GoToAsync("//SettingsPage");
}