namespace Pomodorek.Services;

public class PopupService : IPopupService
{
    public void ClosePopup(Popup popup) => popup?.Close();

    public SessionDetailsPopup ShowSessionDetailsPopup()
    {
        var popup = new SessionDetailsPopup();

        Application.Current?.MainPage?.ShowPopup(popup);

        return popup;
    }
}