namespace Pomodorek.Services;

public interface IPomodorekPopupService
{
    /// <summary>
    /// Close popup dialog.
    /// </summary>
    /// <param name="popup"><see cref="Popup" /> object to be closed.</param>
    void ClosePopup(Popup? popup);

    /// <summary>
    /// Display popup dialog with session details.
    /// </summary>
    /// <returns><see cref="SessionDetailsPopup" /> object to be displayed.</returns>
    SessionDetailsPopup ShowSessionDetailsPopup();
}