﻿namespace Pomodorek.Interfaces;

public interface INavigationService
{
    /// <summary>
    /// Navigate to the timer page.
    /// </summary>
    Task GoToTimerPageAsync();

    /// <summary>
    /// Navigate to the settings page.
    /// </summary>
    Task GoToSettingsPageAsync();
}