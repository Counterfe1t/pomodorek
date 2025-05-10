namespace Pomodorek.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    private readonly IBrowser _browser;

    [ObservableProperty]
    private string _appName;

    [ObservableProperty]
    private string _appVersion;

    public AboutPageViewModel(
        IConfigurationService configurationService,
        INavigationService navigationService,
        IBrowser browser)
        : base(AppResources.AboutPage_Title, navigationService)
    {
        _appName = configurationService.AppSettings.AppName;
        _appVersion = configurationService.AppSettings.AppVersion;
        _browser = browser;
    }

    [RelayCommand]
    private async Task GoToUrl(string url)
        => await _browser.OpenAsync(new Uri(url), new BrowserLaunchOptions());
}