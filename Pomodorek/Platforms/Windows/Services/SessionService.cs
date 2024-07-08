using Pomodorek.Interfaces;

namespace Pomodorek.Services;

public class SessionService : BaseSessionService
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

    public override void StartInterval(SessionModel session) => PlaySound(Constants.Sounds.IntervalStart);

    public override void FinishInterval(SessionModel session)
    {
        PlaySound(Constants.Sounds.IntervalOver);

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

    private void DisplayNotification(string content) =>
        Task.Run(async () => await _notificationService.DisplayNotificationAsync(new NotificationModel
        {
            Content = content
        }));

    private void FinishWorkInterval(SessionModel session)
    {
        session.WorkIntervalsCount++;

        if (session.IsLongRest)
        {
            session.CurrentInterval = IntervalEnum.LongRest;
            DisplayNotification(Constants.Messages.LongRest);
            return;
        }

        session.CurrentInterval = IntervalEnum.ShortRest;
        DisplayNotification(Constants.Messages.ShortRest);
    }

    private void FinishRestInterval(SessionModel session)
    {
        if (session.CurrentInterval == IntervalEnum.ShortRest)
            session.ShortRestIntervalsCount++;
        else
            session.LongRestIntervalsCount++;

        session.CurrentInterval = IntervalEnum.Work;
        DisplayNotification(Constants.Messages.Work);
    }
}