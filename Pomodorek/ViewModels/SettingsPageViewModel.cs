using System.Windows.Input;

namespace Pomodorek.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private bool _isSoundEnabled;
    private int _focusLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    #region Properties
    
    public bool IsSoundEnabled
    {
        get => _isSoundEnabled;
        set => SetProperty(ref _isSoundEnabled, value);
    }

    public int FocusLengthInMin
    {
        get => _focusLengthInMin;
        set => SetProperty(ref _focusLengthInMin, value);
    }

    public int ShortRestLengthInMin
    {
        get => _shortRestLengthInMin;
        set => SetProperty(ref _shortRestLengthInMin, value);
    }

    public int LongRestLengthInMin
    {
        get => _longRestLengthInMin;
        set => SetProperty(ref _longRestLengthInMin, value);
    }

    #endregion

    private readonly ISettingsService _settingsService;
    private readonly IConfigurationService _configurationService;
    private readonly IAlertService _alertService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public ICommand InitializeCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService,
        IAlertService alertService)
    {
        Title = Constants.PageTitles.Settings;
        _settingsService = settingsService;
        _configurationService = configurationService;
        _alertService = alertService;

        InitializeCommand = new Command(Initialize);
        SaveCommand = new Command(() =>
        {
            SaveSettings();
            _alertService.DisplayAlert(Constants.PageTitles.Settings, Constants.Messages.SettingsSaved);
        });
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
    }

    // TODO: Save settings only if any changes have been made
    private void SaveSettings()
    {
        _settingsService.Set(Constants.Settings.IsSoundEnabled, IsSoundEnabled);
        _settingsService.Set(Constants.Settings.FocusLengthInMin, FocusLengthInMin);
        _settingsService.Set(Constants.Settings.ShortRestLengthInMin, ShortRestLengthInMin);
        _settingsService.Set(Constants.Settings.LongRestLengthInMin, LongRestLengthInMin);
    }
}