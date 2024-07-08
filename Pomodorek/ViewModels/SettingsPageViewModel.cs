using Pomodorek.Interfaces;

namespace Pomodorek.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _isChangePending;

    private bool _isDarkThemeEnabled;
    private bool _isSoundEnabled;
    private float _soundVolume;
    private int _workLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    public bool IsDarkThemeEnabled
    {
        get => _isDarkThemeEnabled;
        set { if (SetProperty(ref _isDarkThemeEnabled, value)) IsChangePending = true; }
    }

    public bool IsSoundEnabled
    {
        get => _isSoundEnabled;
        set { if (SetProperty(ref _isSoundEnabled, value)) IsChangePending = true; }
    }

    public float SoundVolume
    {
        get => _soundVolume;
        set { if (SetProperty(ref _soundVolume, value)) IsChangePending = true; }
    }

    public int WorkLengthInMin
    {
        get => _workLengthInMin;
        set { if (SetProperty(ref _workLengthInMin, value)) IsChangePending = true; }
    }

    public int ShortRestLengthInMin
    {
        get => _shortRestLengthInMin;
        set { if (SetProperty(ref _shortRestLengthInMin, value)) IsChangePending = true; }
    }

    public int LongRestLengthInMin
    {
        get => _longRestLengthInMin;
        set { if (SetProperty(ref _longRestLengthInMin, value)) IsChangePending = true; }
    }

    private readonly ISettingsService _settingsService;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;

    private readonly AppSettings _appSettings;
    private readonly Application _application;

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService,
        IAlertService alertService,
        INavigationService navigationService,
        IApplicationService applicationService)
        : base(Constants.Pages.Settings)
    {
        _appSettings = configurationService.AppSettings;
        _application = applicationService.Application;
        _settingsService = settingsService;
        _alertService = alertService;
        _navigationService = navigationService;
    }

    public void InitializeSettings()
    {
        IsDarkThemeEnabled = _settingsService.Get(Constants.Settings.IsDarkThemeEnabled, _appSettings.DefaultIsDarkThemeEnabled);
        IsSoundEnabled = _settingsService.Get(Constants.Settings.IsSoundEnabled, _appSettings.DefaultIsSoundEnabled);
        SoundVolume = _settingsService.Get(Constants.Settings.SoundVolume, _appSettings.DefaultSoundVolume);
        WorkLengthInMin = _settingsService.Get(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin);
        ShortRestLengthInMin = _settingsService.Get(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin);
        LongRestLengthInMin = _settingsService.Get(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin);
        IsChangePending = false;
    }

    // TODO Add simple validation (rest duration cannot be longer than focus duration)
    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        _application.UserAppTheme = IsDarkThemeEnabled
            ? AppTheme.Dark
            : AppTheme.Light;

        _settingsService.Set(Constants.Settings.IsDarkThemeEnabled, IsDarkThemeEnabled);
        _settingsService.Set(Constants.Settings.IsSoundEnabled, IsSoundEnabled);
        _settingsService.Set(Constants.Settings.SoundVolume, SoundVolume);
        _settingsService.Set(Constants.Settings.WorkLengthInMin, WorkLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, ShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, LongRestLengthInMin);
        IsChangePending = false;

        await _alertService.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsSaved);
        await _navigationService.GoToTimerPageAsync();
    }

    [RelayCommand]
    private async Task RestoreSettingsAsync()
    {
        if (!await _alertService.DisplayConfirmAsync(Title, Constants.Messages.RestoreDefaultSettings))
            return;

        _application.UserAppTheme = AppTheme.Light;

        IsDarkThemeEnabled = _appSettings.DefaultIsDarkThemeEnabled;
        IsSoundEnabled = _appSettings.DefaultIsSoundEnabled;
        SoundVolume = _appSettings.DefaultSoundVolume;
        WorkLengthInMin = _appSettings.DefaultWorkLengthInMin;
        ShortRestLengthInMin = _appSettings.DefaultShortRestLengthInMin;
        LongRestLengthInMin = _appSettings.DefaultLongRestLengthInMin;

        IsChangePending = false;

        _settingsService.Set(Constants.Settings.IsDarkThemeEnabled, _appSettings.DefaultIsDarkThemeEnabled);
        _settingsService.Set(Constants.Settings.IsSoundEnabled, _appSettings.DefaultIsSoundEnabled);
        _settingsService.Set(Constants.Settings.SoundVolume, _appSettings.DefaultSoundVolume);
        _settingsService.Set(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin);

        await _alertService.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsRestored);
    }
}