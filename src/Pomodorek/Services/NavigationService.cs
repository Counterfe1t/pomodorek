
namespace Pomodorek.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToTimerPageAsync()
         => await Shell.Current.GoToAsync($"//{Constants.Routes.TimerPage}");

    public async Task NavigateToSettingsPageAsync()
        => await Shell.Current.GoToAsync($"//{Constants.Routes.SettingsPage}");
}