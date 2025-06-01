namespace Pomodorek.Services;

public class AlertService : IAlertService
{
    private readonly Application? _application;

    public AlertService(IApplicationService applicationService)
    {
        _application = applicationService.Application;
    }

    public async Task DisplayAlertAsync(string title, string message)
    {
        if (_application?.MainPage is not Page page)
            return;

        await page.DisplayAlert(
            title,
            message,
            AppResources.Common_Okay);
    }

    public async Task<bool> DisplayConfirmAsync(string title, string message)
    {
        if (_application?.MainPage is not Page page)
            return false;

        return await page.DisplayAlert(
            title,
            message,
            AppResources.Common_Yes,
            AppResources.Common_No);
    }
}