namespace Pomodorek.Services.Interfaces;

public interface IPopupService
{
    /// <summary>
    /// Display popup dialog.
    /// </summary>
    /// <param name="popup"><see cref="Popup" /> object to be displayed.</param>
    void ShowPopup(Popup popup);
}