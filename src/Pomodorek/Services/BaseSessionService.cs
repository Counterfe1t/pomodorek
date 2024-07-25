namespace Pomodorek.Services;

public abstract class BaseSessionService : ISessionService
{
    private readonly ISettingsService _settingsService;
    private readonly ISoundService _soundService;

    private readonly AppSettings _appSettings;

    public static SessionModel GetNewSession => new()
    {
        IntervalsCount = 0,
        WorkIntervalsCount = 0,
        ShortRestIntervalsCount = 0,
        LongRestIntervalsCount = 0,
        CurrentInterval = IntervalEnum.Work
    };

    public BaseSessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService)
    {
        _appSettings = configurationService.AppSettings;
        _settingsService = settingsService;
        _soundService = soundService;
    }

    public SessionModel GetSession()
    {
        string serializedSession = _settingsService.Get(Constants.Settings.SavedSession, string.Empty);
        if (string.IsNullOrWhiteSpace(serializedSession))
            return GetNewSession;

        return JsonSerializer.Deserialize<SessionModel>(serializedSession);
    }

    public void SaveSession(SessionModel session) =>
        _settingsService.Set(Constants.Settings.SavedSession, JsonSerializer.Serialize(session));

    public abstract void StartInterval(SessionModel session);

    public abstract void FinishInterval(SessionModel session);

    public int GetIntervalLengthInSec(IntervalEnum interval) =>
        GetIntervalLengthInMin(interval) * Constants.OneMinuteInSec;

    private int GetIntervalLengthInMin(IntervalEnum interval) =>
        interval switch
        {
            IntervalEnum.Work =>
                _settingsService.Get(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin),
            IntervalEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin),
            IntervalEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin),
            _ => 0
        };

    protected void PlaySound(string fileName) =>
        Task.Run(async () => await _soundService.PlaySoundAsync(fileName));
}