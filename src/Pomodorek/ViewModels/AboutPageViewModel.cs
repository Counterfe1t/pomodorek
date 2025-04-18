namespace Pomodorek.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _appName;

    [ObservableProperty]
    private string _appVersion;

    public AboutPageViewModel(IConfigurationService configurationService)
        : base(Constants.Pages.About)
    {
        _appName = configurationService.AppSettings.AppName;
        _appVersion = configurationService.AppSettings.AppVersion;
    }

    [RelayCommand]
    private async Task GoToUrl(string url)
        => await Launcher.OpenAsync(new Uri(url));
}