using Microsoft.Maui.Handlers;

namespace Pomodorek;

public partial class App : Application
{
    private readonly ISettingsService _settingsService;

    private readonly AppSettings _appSettings;

    public App(
        ISettingsService settingsService,
        IConfigurationService configurationService)
    {
        InitializeComponent();
        InitializeHandlers();

        MainPage = new AppShell();

        _appSettings = configurationService.AppSettings;
        _settingsService = settingsService;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        // Adjust window size.
        if (DeviceInfo.Platform == DevicePlatform.WinUI ||
            DeviceInfo.Platform == DevicePlatform.MacCatalyst)
        {
            window.Width = 800;
            window.MinimumWidth = 800;
            window.MaximumWidth = 1920;
            window.Height= 600;
            window.MinimumHeight = 600;
            window.MaximumHeight = 1080;
        }

        // Load application theme from local settings.
        UserAppTheme = _settingsService.Get(Constants.Settings.IsDarkThemeEnabled, _appSettings.DefaultIsDarkThemeEnabled)
            ? AppTheme.Dark
            : AppTheme.Light;

        return window;
    }

    private void InitializeHandlers()
    {
#if WINDOWS
        SwitchHandler.Mapper.AppendToMapping("CustomLabelSwitch", (handler, control) =>
        {
            handler.PlatformView.OnContent = null;
            handler.PlatformView.OffContent = null;
        });
#endif
    }
}