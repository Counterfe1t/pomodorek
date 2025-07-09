namespace Pomodorek.Services;

public class SessionService : SessionServiceBase
{
    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService)
        : base(
            configurationService,
            settingsService,
            soundService)
    {
        // TODO Initialize required services
    }

    public override void StartInterval(SessionModel session)
        => PlaySound(AppResources.Common_IntervalStartFileName);

    public override void FinishInterval(SessionModel session)
        => PlaySound(AppResources.Common_IntervalOverFileName);
}