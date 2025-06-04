namespace Pomodorek.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IAlertService _alertService;

    private readonly AppSettings _appSettings;
    private readonly Application _application;

    private bool _isChangePending;
    private bool _isDarkThemeEnabled;
    private bool _isSoundEnabled;
    private float _soundVolume;
    private int _workLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    public bool IsChangePending
    {
        get => _isChangePending;
        set => SetProperty(ref _isChangePending, value);
    }

    public bool IsDarkThemeEnabled
    {
        get => _isDarkThemeEnabled;
        set
        {
            SetProperty(ref _isDarkThemeEnabled, value);
            SetAppTheme(value);
        }
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

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService,
        IAlertService alertService,
        INavigationService navigationService,
        IApplicationService applicationService)
        : base(AppResources.SettingsPage_Title, navigationService)
    {
        _appSettings = configurationService.AppSettings;
        _application = applicationService.Application;
        _settingsService = settingsService;
        _alertService = alertService;
    }

    public void InitializeSettings()
    {
        // Get saved settings from devide preferences.
        IsDarkThemeEnabled = _settingsService.Get(Constants.Settings.IsDarkThemeEnabled, _appSettings.DefaultIsDarkThemeEnabled);
        IsSoundEnabled = _settingsService.Get(Constants.Settings.IsSoundEnabled, _appSettings.DefaultIsSoundEnabled);
        SoundVolume = _settingsService.Get(Constants.Settings.SoundVolume, _appSettings.DefaultSoundVolume);
        WorkLengthInMin = _settingsService.Get(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin);
        ShortRestLengthInMin = _settingsService.Get(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin);
        LongRestLengthInMin = _settingsService.Get(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin);

        // There are no pending changes.
        IsChangePending = false;
    }

    public async Task<bool> DisplayUnsavedChangesDialog()
        => await _alertService.DisplayConfirmAsync(Title, Constants.Messages.UnsavedChanges);

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        if (!await ValidateSettings())
            return;

        // Save settings to device preferences.
        _settingsService.Set(Constants.Settings.IsDarkThemeEnabled, IsDarkThemeEnabled);
        _settingsService.Set(Constants.Settings.IsSoundEnabled, IsSoundEnabled);
        _settingsService.Set(Constants.Settings.SoundVolume, SoundVolume);
        _settingsService.Set(Constants.Settings.WorkLengthInMin, WorkLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, ShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, LongRestLengthInMin);

        // There are no pending changes.
        IsChangePending = false;

        await _alertService.DisplayAlertAsync(AppResources.SettingsPage_Title, Constants.Messages.SettingsSaved);
        await _navigationService.NavigateToAsync(AppResources.TimerPage_Route);
    }

    [RelayCommand]
    private async Task RestoreSettingsAsync()
    {
        // Prompt user with confirm dialog before restoring settings to default.
        if (!await _alertService.DisplayConfirmAsync(Title, Constants.Messages.RestoreDefaultSettings))
            return;

        // Set settings to default values.
        IsDarkThemeEnabled = _appSettings.DefaultIsDarkThemeEnabled;
        IsSoundEnabled = _appSettings.DefaultIsSoundEnabled;
        SoundVolume = _appSettings.DefaultSoundVolume;
        WorkLengthInMin = _appSettings.DefaultWorkLengthInMin;
        ShortRestLengthInMin = _appSettings.DefaultShortRestLengthInMin;
        LongRestLengthInMin = _appSettings.DefaultLongRestLengthInMin;

        // There are no pending changes.
        IsChangePending = false;

        // Save settings to device preferences.
        _settingsService.Set(Constants.Settings.IsSoundEnabled, _appSettings.DefaultIsSoundEnabled);
        _settingsService.Set(Constants.Settings.SoundVolume, _appSettings.DefaultSoundVolume);
        _settingsService.Set(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin);

        await _alertService.DisplayAlertAsync(AppResources.SettingsPage_Title, Constants.Messages.SettingsRestored);
    }

    private async Task<bool> ValidateSettings()
    {
        if (WorkLengthInMin < ShortRestLengthInMin)
        {
            await _alertService.DisplayAlertAsync(
                AppResources.SettingsPage_Title,
                AppResources.SettingsPage_WorkShorterThanRestText);

            return false;
        }

        if (LongRestLengthInMin < ShortRestLengthInMin)
        {
            await _alertService.DisplayAlertAsync(
                AppResources.SettingsPage_Title,
                AppResources.SettingsPage_LongRestShorterThanShortRestText);

            return false;
        }

        // Validation was successful.
        return true;
    }

    private void SetAppTheme(bool isDarkThemeEnabled)
    {
        _settingsService.Set(Constants.Settings.IsDarkThemeEnabled, isDarkThemeEnabled);
        _application.UserAppTheme = isDarkThemeEnabled
            ? AppTheme.Dark
            : AppTheme.Light;
    }
}