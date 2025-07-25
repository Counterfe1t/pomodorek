﻿namespace Pomodorek.Models;

public static class Constants
{
    public const int OneMinuteInSec = 60;
    public const int OneSecondInTicks = 10_000_000;
    public const int OneSecondInMs = 1_000;

    public const string TimeFormat = "HH:mm";

    public class Settings
    {
        public const string IsDarkThemeEnabled = nameof(IsDarkThemeEnabled);
        public const string IsSoundEnabled = nameof(IsSoundEnabled);
        public const string SoundVolume = nameof(SoundVolume);
        public const string WorkLengthInMin = nameof(WorkLengthInMin);
        public const string ShortRestLengthInMin = nameof(ShortRestLengthInMin);
        public const string LongRestLengthInMin = nameof(LongRestLengthInMin);
        public const string IntervalsCount = nameof(IntervalsCount);
        public const string SavedSession = nameof(SavedSession);
    }
}