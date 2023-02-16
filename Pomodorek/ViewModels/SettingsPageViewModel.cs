using System.Windows.Input;

namespace Pomodorek.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private int _focusLengthInMin;
    private int _shortRestLengthInMin;
    private int _longRestLengthInMin;

    #region Properties

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

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public ICommand InitializeCommand { get; private set; }
    public ICommand SaveCommand { get; private set; }

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfigurationService configurationService)
    {
        Title = Constants.PageTitles.Settings;
        _settingsService = settingsService;
        _configurationService = configurationService;

        InitializeCommand = new Command(Initialize);
        SaveCommand = new Command(SaveSettings);
    }

    private void Initialize()
    {
        FocusLengthInMin =
            _settingsService.Get(Constants.FocusLengthInMin, AppSettings.DefaultFocusLengthInMin);
        ShortRestLengthInMin =
            _settingsService.Get(Constants.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin);
        LongRestLengthInMin =
            _settingsService.Get(Constants.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin);
    }

    private void SaveSettings()
    {
        _settingsService.Set(Constants.FocusLengthInMin, FocusLengthInMin);
        _settingsService.Set(Constants.ShortRestLengthInMin, ShortRestLengthInMin);
        _settingsService.Set(Constants.LongRestLengthInMin, LongRestLengthInMin);
    }
}
