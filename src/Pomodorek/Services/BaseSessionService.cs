namespace Pomodorek.Services;

public abstract class BaseSessionService : ISessionService
{
    private readonly ISettingsService _settingsService;
    private readonly ISoundService _soundService;

    private readonly AppSettings _appSettings;

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
            return SessionModel.Create();

        return JsonSerializer.Deserialize<SessionModel>(serializedSession);
    }

    public abstract void StartInterval(SessionModel session);

    public abstract void FinishInterval(SessionModel session);

    public void SaveSession(SessionModel session)
        => _settingsService.Set(Constants.Settings.SavedSession, JsonSerializer.Serialize(session));

    public int GetIntervalLengthInSec(IntervalEnum interval)
        => GetIntervalLengthInMin(interval) * Constants.OneMinuteInSec;

    private int GetIntervalLengthInMin(IntervalEnum interval)
        => interval switch
        {
            IntervalEnum.Work =>
                _settingsService.Get(Constants.Settings.WorkLengthInMin, _appSettings.DefaultWorkLengthInMin),
            IntervalEnum.ShortRest =>
                _settingsService.Get(Constants.Settings.ShortRestLengthInMin, _appSettings.DefaultShortRestLengthInMin),
            IntervalEnum.LongRest =>
                _settingsService.Get(Constants.Settings.LongRestLengthInMin, _appSettings.DefaultLongRestLengthInMin),
            _ => 0
        };

    protected void PlaySound(string fileName)
        => Task.Run(async () => await _soundService.PlaySoundAsync(fileName));
}