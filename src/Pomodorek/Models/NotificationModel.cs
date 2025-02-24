﻿namespace Pomodorek.Models;

public class NotificationModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public bool IsOngoing { get; set; }

    public bool OnlyAlertOnce { get; set; }

    public int MaxProgress { get; set; }

    public int CurrentProgress { get; set; }

    public DateTimeOffset TriggerAlarmAt { get; set; }
}