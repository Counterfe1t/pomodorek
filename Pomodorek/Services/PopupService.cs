namespace Pomodorek.Services;

public class PopupService : IPopupService
{
    // TODO: Unit tests missing
    public void ShowPopup(Popup popup) => Application.Current?.MainPage?.ShowPopup(popup);
}