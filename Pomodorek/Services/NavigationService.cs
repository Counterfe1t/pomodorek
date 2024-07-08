using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class NavigationService : INavigationService
{
    public async Task GoToSettingsPageAsync() => await Shell.Current.GoToAsync("//SettingsPage");

    public async Task GoToTimerPageAsync() => await Shell.Current.GoToAsync("//TimerPage");
}