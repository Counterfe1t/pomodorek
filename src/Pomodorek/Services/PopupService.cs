namespace Pomodorek.Services;

public class PopupService : IPopupService
{
    private readonly Application _application;

    public PopupService(IApplicationService applicationService)
    {
        _application = applicationService.Application;
    }

    public void ClosePopup(Popup popup) => popup?.Close();

    public SessionDetailsPopup ShowSessionDetailsPopup()
    {
        var popup = new SessionDetailsPopup();

        _application?.MainPage?.ShowPopup(popup);

        return popup;
    }
}