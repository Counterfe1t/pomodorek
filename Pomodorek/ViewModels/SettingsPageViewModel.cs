using Microsoft.Extensions.Configuration;
using Pomodorek.Models;
using Pomodorek.Resources.Constants;
using Pomodorek.Services;
using System.Windows.Input;

namespace Pomodorek.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;
    private readonly IConfiguration _configuration;

    private AppSettings AppSettings => _configuration.Get<AppSettings>();

    #region Properties

    private int _focusLengthInMin;
    public int FocusLengthInMin
    {
        get => _focusLengthInMin;
        set => SetProperty(ref _focusLengthInMin, value);
    }

    private int _shortRestLengthInMin;
    public int ShortRestLengthInMin
    {
        get => _shortRestLengthInMin;
        set => SetProperty(ref _shortRestLengthInMin, value);
    }

    private int _longRestLengthInMin;
    public int LongRestLengthInMin
    {
        get => _longRestLengthInMin;
        set => SetProperty(ref _longRestLengthInMin, value);
    }

    #endregion

    public ICommand SaveCommand { get; private set; }
    public ICommand InitializeCommand { get; private set; }

    public SettingsPageViewModel(
        ISettingsService settingsService,
        IConfiguration configuration)
    {
        Title = Constants.PageTitles.Settings;
        _settingsService = settingsService;
        _configuration = configuration;
        SaveCommand = new Command(SaveSettings);
        InitializeCommand = new Command(Initialize);
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
