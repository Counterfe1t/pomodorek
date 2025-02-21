namespace Pomodorek.Services;

public class AlertService : IAlertService
{
    private readonly Application _application;

    public AlertService(IApplicationService applicationService)
    {
        _application = applicationService.Application;
    }

    public async Task DisplayAlertAsync(string title, string message)
        => await _application?.MainPage?.DisplayAlert(title, message, Constants.Messages.Confirm);

    public async Task<bool> DisplayConfirmAsync(string title, string message)
        => await _application?.MainPage?.DisplayAlert(
            title,
            message,
            Constants.Messages.Confirm,
            Constants.Messages.Cancel);
}