namespace Pomodorek.Services;

public class SessionService : BaseSessionService
{
    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService) : base(
            configurationService,
            settingsService,
            soundService)
    {
    }

    public override void StartInterval(SessionModel session)
    {
        PlaySound(Constants.Sounds.IntervalStart);
    }

    public override void FinishInterval(SessionModel session)
    {
        PlaySound(Constants.Sounds.IntervalOver);
    }
}