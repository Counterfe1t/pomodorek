namespace Pomodorek.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private bool _isChangePending;
    private bool _isSoundEnabled;
    private int _focusLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    #region Properties

    public bool IsChangePending
    {
        get => _isChangePending;
        set => SetProperty(ref _isChangePending, value);
    }

    public bool IsSoundEnabled
    {
        get => _isSoundEnabled;
        set
        {
            IsChangePending = true;
            SetProperty(ref _isSoundEnabled, value);
        }
    }

    public int FocusLengthInMin
    {
        get => _focusLengthInMin;
        set
        {
            IsChangePending = true;
            SetProperty(ref _focusLengthInMin, value);
        }
    }

    public int ShortRestLengthInMin
    {
        get => _shortRestLengthInMin;
        set
        {
            IsChangePending = true;
            SetProperty(ref _shortRestLengthInMin, value);
        }
    }

    public int LongRestLengthInMin
    {
        get => _longRestLengthInMin;
        set
        {
            IsChangePending = true;
            SetProperty(ref _longRestLengthInMin, value);
        }
    }

    #endregion

    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public ICommand InitializeCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService,
        IAlertService alertService,
        INavigationService navigationService)
    {
        Title = Constants.Pages.Settings;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _alertService = alertService;
        _navigationService = navigationService;

        InitializeCommand = new Command(Initialize);
        SaveCommand = new Command(async () => await SaveSettings());
    }

    private void Initialize()
    {
        IsSoundEnabled = _settingsService.Get(Constants.Settings.IsSoundEnabled, true);

        FocusLengthInMin = _settingsService.Get(
            Constants.Settings.FocusLengthInMin,
            AppSettings.DefaultFocusLengthInMin);

        ShortRestLengthInMin = _settingsService.Get(
            Constants.Settings.ShortRestLengthInMin,
            AppSettings.DefaultShortRestLengthInMin);

        LongRestLengthInMin = _settingsService.Get(
            Constants.Settings.LongRestLengthInMin,
            AppSettings.DefaultLongRestLengthInMin);

        IsChangePending = false;
    }

    // TODO: Add simple validation (rest duration cannot be longer than focus duration)
    private async Task SaveSettings()
    {
        _settingsService.Set(Constants.Settings.IsSoundEnabled, IsSoundEnabled);
        _settingsService.Set(Constants.Settings.FocusLengthInMin, FocusLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, ShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, LongRestLengthInMin);
        IsChangePending = false;

        await _alertService.DisplayAlertAsync(Constants.Pages.Settings, Constants.Messages.SettingsSaved);
        await _navigationService.GoToTimerPageAsync();
    }
}