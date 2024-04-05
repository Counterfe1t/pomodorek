namespace Pomodorek.Services;

public interface IAlertService
{
    Task DisplayAlertAsync(string title, string message);

    Task<bool> DisplayConfirmAsync(string title, string message);
}