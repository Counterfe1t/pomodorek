namespace Pomodorek.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAlertAsync(string title, string message) =>
        await Application.Current?.MainPage?.DisplayAlert(title, message, Constants.Messages.Confirm);

    public async Task<bool> DisplayConfirmAsync(string title, string message) =>
        await Application.Current?.MainPage?.DisplayAlert(
            title,
            message,
            Constants.Messages.Confirm,
            Constants.Messages.Cancel);
}