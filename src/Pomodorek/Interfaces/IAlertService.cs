namespace Pomodorek.Interfaces;

public interface IAlertService
{
    /// <summary>
    /// Display simple dialog box with informative content.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="message">Dialog message content.</param>
    Task DisplayAlertAsync(string title, string message);

    /// <summary>
    /// Display dialog box requesting confirmation from user.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="message">Dialog message content.</param>
    /// <returns>Returns true if user confirmed the action. Otherwise returns false.</returns>
    Task<bool> DisplayConfirmAsync(string title, string message);
}