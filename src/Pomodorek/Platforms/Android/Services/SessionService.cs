﻿namespace Pomodorek.Services;

public class SessionService : SessionServiceBase
{
    private readonly ISettingsService _settingsService;

    public SessionService(
        IConfigurationService configurationService,
        ISettingsService settingsService,
        ISoundService soundService)
        : base(
            configurationService,
            settingsService,
            soundService)
    {
        _settingsService = settingsService;
    }

    public override void StartInterval(SessionModel session)
    {
        PlaySound(AppResources.Common_IntervalStartFileName);

        _settingsService.Set(
            nameof(NotificationModel),
            JsonSerializer.Serialize(new NotificationModel
            {
                Id = 2137,
                Title = GetIntervalTitle(session.CurrentInterval),
                Content = GetIntervalFinishedMessage(session),
                TriggerAlarmAt = session.TriggerAlarmAt,
                MaxProgress = GetIntervalLengthInSec(session.CurrentInterval)
            }));
    }

    public override void FinishInterval(SessionModel session)
    {
        PlaySound(AppResources.Common_IntervalOverFileName);

        session.IntervalsCount++;
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
                session.CurrentInterval = IntervalEnum.Work;
                break;
        }
    }

    private string GetIntervalFinishedMessage(SessionModel session)
    {
        var notificationTime = session.TriggerAlarmAt.ToLocalTime().ToString("HH:mm");

        if (session.CurrentInterval != IntervalEnum.Work)
            return StringParser.Parse(notificationTime, Constants.Messages.Work);

        return session.WorkIntervalsCount % 4 == 3
            ? StringParser.Parse(notificationTime, Constants.Messages.LongRest)
            : StringParser.Parse(notificationTime, Constants.Messages.ShortRest);
    }

    private string GetIntervalTitle(IntervalEnum interval)
        => interval switch
        {
            IntervalEnum.Work => AppResources.TimerPage_WorkIntervalLabel,
            IntervalEnum.ShortRest => AppResources.TimerPage_ShortRestIntervalLabel,
            IntervalEnum.LongRest => AppResources.TimerPage_LongRestIntervalLabel,
            _ => string.Empty
        };
}