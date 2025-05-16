namespace Pomodorek.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    protected readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy = false;

    public bool IsNotBusy => !IsBusy;

    public BaseViewModel(string title, INavigationService navigationService)
    {
        Title = title;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task NavigateToTimerPageAsync()
        => await _navigationService.NavigateToAsync(AppResources.TimerPage_Route);

    [RelayCommand]
    private async Task NavigateToSettingsPageAsync()
        => await _navigationService.NavigateToAsync(AppResources.SettingsPage_Route);

    [RelayCommand]
    private async Task NavigateToAboutPageAsync()
        => await _navigationService.NavigateToAsync(AppResources.AboutPage_Route);
}