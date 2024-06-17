namespace Pomodorek.ViewModels;

public partial class SettingsPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _isChangePending;

    private bool _isSoundEnabled;
    private float _soundVolume;
    private int _workLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    public bool IsSoundEnabled
    {
        get => _isSoundEnabled;
        set
        {
            if (SetProperty(ref _isSoundEnabled, value))
                IsChangePending = true;
        }
    }

    public float SoundVolume
    {
        get => _soundVolume;
        set
        {
            if (SetProperty(ref _soundVolume, value))
                IsChangePending = true;
        }
    }

    public int WorkLengthInMin
    {
        get => _workLengthInMin;
        set
        {
            if (SetProperty(ref _workLengthInMin, value))
                IsChangePending = true;
        }
    }

    public int ShortRestLengthInMin
    {
        get => _shortRestLengthInMin;
        set
        {
            if (SetProperty(ref _shortRestLengthInMin, value))
                IsChangePending = true;
        }
    }

    public int LongRestLengthInMin
    {
        get => _longRestLengthInMin;
        set
        {
            if (SetProperty(ref _longRestLengthInMin, value))
                IsChangePending = true;
        }
    }

    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService,
        IAlertService alertService,
        INavigationService navigationService)
        : base(Constants.Pages.Settings)
    {
        _settingsService = settingsService;
        _configurationService = configurationService;
        _alertService = alertService;
        _navigationService = navigationService;
    }

    public void InitializeSettings()
    {
        IsSoundEnabled = _settingsService.Get(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled);
        SoundVolume = _settingsService.Get(Constants.Settings.SoundVolume, AppSettings.DefaultSoundVolume);
        WorkLengthInMin = _settingsService.Get(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin);
        ShortRestLengthInMin = _settingsService.Get(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin);
        LongRestLengthInMin = _settingsService.Get(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin);
        IsChangePending = false;
    }

    // TODO Add simple validation (rest duration cannot be longer than focus duration)
    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
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

        IsSoundEnabled = AppSettings.DefaultIsSoundEnabled;
        SoundVolume = AppSettings.DefaultSoundVolume;
        WorkLengthInMin = AppSettings.DefaultWorkLengthInMin;
        ShortRestLengthInMin = AppSettings.DefaultShortRestLengthInMin;
        LongRestLengthInMin = AppSettings.DefaultLongRestLengthInMin;
        IsChangePending = false;

        _settingsService.Set(Constants.Settings.IsSoundEnabled, AppSettings.DefaultIsSoundEnabled);
        _settingsService.Set(Constants.Settings.SoundVolume, AppSettings.DefaultSoundVolume);
        _settingsService.Set(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin);

        await _alertService.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsRestored);
    }
}