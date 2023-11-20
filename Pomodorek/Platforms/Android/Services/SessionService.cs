namespace Pomodorek.Services;

public class SessionService : BaseSessionService, ISessionService
{
    private readonly ISettingsService _settingsService;

    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService)
        : base(configurationService, settingsService, soundService)
    {
        _settingsService = settingsService;
    }

    public void StartInterval(Session session)
    {
        PlaySound(Constants.Sounds.SessionStart);
        _settingsService.Set(nameof(Notification), JsonSerializer.Serialize(new Notification
        {
            Id = 2137,
            Title = session.CurrentInterval.ToString(),
            TriggerAlarmAt = session.TriggerAlarmAt,
            MaxProgress = GetIntervalLengthInSec(session.CurrentInterval),
            IsOngoing = true,
            OnlyAlertOnce = true
        }));
    }

    public void FinishInterval(Session session)
    {
        session.IntervalsCount++;
        PlaySound(Constants.Sounds.SessionOver);

        switch (session.CurrentInterval)
        {
            case IntervalEnum.Work:
                session.WorkIntervalsCount++;

                if (session.IsLongRest)
                {
                    session.CurrentInterval = IntervalEnum.LongRest;
                    break;
                }

                session.CurrentInterval = IntervalEnum.ShortRest;
                break;
            case IntervalEnum.ShortRest:
                session.ShortRestIntervalsCount++;
                session.CurrentInterval = IntervalEnum.Work;
                break;
            case IntervalEnum.LongRest:
                session.LongRestIntervalsCount++;
                session.CurrentInterval = IntervalEnum.Work;
                break;
            default:
                break;
        }
    }
}