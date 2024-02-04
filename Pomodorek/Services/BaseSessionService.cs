namespace Pomodorek.Services;

public class BaseSessionService
{
    private readonly IConfigurationService _configurationService;
    private readonly ISettingsService _settingsService;
    private readonly ISoundService _soundService;

    private AppSettings AppSettings => _configurationService.GetAppSettings();

    public BaseSessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService)
    {
        _configurationService = configurationService;
        _settingsService = settingsService;
        _soundService = soundService;
    }

    public static SessionModel GetNewSession() => new() { CurrentInterval = IntervalEnum.Work };

    public int GetIntervalLengthInMin(IntervalEnum interval) =>
        interval switch
        {
            IntervalEnum.Work =>
                _settingsService.Get(Constants.Settings.WorkLengthInMin, AppSettings.DefaultWorkLengthInMin),
            IntervalEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, AppSettings.DefaultShortRestLengthInMin),
            IntervalEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, AppSettings.DefaultLongRestLengthInMin),
            _ => 0
        };

    public int GetIntervalLengthInSec(IntervalEnum interval) => GetIntervalLengthInMin(interval) * Constants.OneMinuteInSec;

    public string GetIntervalFinishedMessage(SessionModel session)
    {
        if (session.CurrentInterval != IntervalEnum.Work)
            return Constants.Messages.Work;

        return session.WorkIntervalsCount % 4 == 3
            ? Constants.Messages.LongRest
            : Constants.Messages.ShortRest;
    }

    public void PlaySound(string fileName) =>
        Task.Run(async () => await _soundService.PlaySoundAsync(fileName));
}