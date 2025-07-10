namespace Pomodorek.Services;

public class SessionService : SessionServiceBase
{
    private readonly INotificationService _notificationService;

    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        INotificationService notificationService,
        ISoundService soundService)
        : base(
            configurationService,
            settingsService,
            soundService)
    {
        _notificationService = notificationService;
    }

    public override void StartInterval(SessionModel session)
        => PlaySound(AppResources.Common_IntervalStartFileName);

    public override void FinishInterval(SessionModel session)
    {
        PlaySound(AppResources.Common_IntervalOverFileName);

        session.IntervalsCount++;
        switch (session.CurrentInterval)
        {
            case IntervalEnum.Work:
                FinishWorkInterval(session);
                break;
            case IntervalEnum.ShortRest:
            case IntervalEnum.LongRest:
                FinishRestInterval(session);
                break;
            default:
                break;
        }
    }

    private void DisplayNotification(string content)
        => MainThread.BeginInvokeOnMainThread(async () =>
            await _notificationService.DisplayNotificationAsync(new NotificationModel
            {
                Title = "Pomodorek",
                Content = content
            }));

    private void FinishWorkInterval(SessionModel session)
    {
        session.WorkIntervalsCount++;

        session.CurrentInterval = session.IsLongRest
            ? IntervalEnum.LongRest
            : IntervalEnum.ShortRest;

        var notificationContent = session.IsLongRest
            ? Constants.Messages.LongRest
            : Constants.Messages.ShortRest;

        DisplayNotification(
            StringParser.Parse(
                session.TriggerAlarmAt.ToLocalTime().ToString(Constants.TimeFormat),
                notificationContent));
    }

    private void FinishRestInterval(SessionModel session)
    {
        if (session.CurrentInterval == IntervalEnum.ShortRest)
            session.ShortRestIntervalsCount++;
        else
            session.LongRestIntervalsCount++;

        session.CurrentInterval = IntervalEnum.Work;

        DisplayNotification(
            StringParser.Parse(
                session.TriggerAlarmAt.ToLocalTime().ToString(Constants.TimeFormat),
                Constants.Messages.Work));
    }
}