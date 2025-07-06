namespace Pomodorek.ViewModels;

public partial class AboutPageViewModel : ViewModelBase
{
    private readonly IBrowser _browser;

    private string _appName = string.Empty;
    private string _appVersion = string.Empty;

    public string AppName
    {
        get => _appName;
        set => SetProperty(ref _appName, value);
    }

    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }

    public AboutPageViewModel(
        IConfigurationService configurationService,
        INavigationService navigationService,
        IBrowser browser)
        : base(AppResources.AboutPage_Title, navigationService)
    {
        AppName = configurationService.AppSettings.AppName;
        AppVersion = configurationService.AppSettings.AppVersion;
        _browser = browser;
    }

    [RelayCommand]
    private async Task GoToUrl(string url)
        => await _browser.OpenAsync(new Uri(url), new BrowserLaunchOptions());
}