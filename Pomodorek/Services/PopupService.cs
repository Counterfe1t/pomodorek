namespace Pomodorek.Services;

public class PopupService : IPopupService
{
    public void ShowPopup(Popup popup) => Application.Current?.MainPage?.ShowPopup(popup);
}