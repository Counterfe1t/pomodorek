namespace Pomodorek.Services;

public class SessionService : BaseSessionService, ISessionService
{
    private readonly INotificationService _notificationService;

    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        INotificationService notificationService,
        ISoundService soundService)
        : base(configurationService, settingsService, soundService)
    {
        _notificationService = notificationService;
    }

    public void StartInterval(Session session)
    {
        PlaySound(Constants.Sounds.SessionStart);
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
                    DisplayNotification(Constants.Messages.LongRest);
                    break;
                }

                session.CurrentInterval = IntervalEnum.ShortRest;
                DisplayNotification(Constants.Messages.ShortRest);
                break;
            case IntervalEnum.ShortRest:
                session.ShortRestIntervalsCount++;
                session.CurrentInterval = IntervalEnum.Work;
                DisplayNotification(Constants.Messages.Focus);
                break;
            case IntervalEnum.LongRest:
                session.LongRestIntervalsCount++;
                session.CurrentInterval = IntervalEnum.Work;
                DisplayNotification(Constants.Messages.Focus);
                break;
            default:
                break;
        }
    }

    private void DisplayNotification(string content) =>
        Task.Run(async () => await _notificationService.DisplayNotificationAsync(new Notification
        {
            Content = content
        }));
}