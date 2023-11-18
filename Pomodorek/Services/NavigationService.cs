namespace Pomodorek.Services;

// TODD: Inject current shell as a dependency
public class NavigationService : INavigationService
{
    public async Task GoToSettingsPageAsync() => await Shell.Current.GoToAsync("//SettingsPage");

    public async Task GoToTimerPageAsync() => await Shell.Current.GoToAsync("//MainPage");
}