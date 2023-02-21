namespace Pomodorek.Services;

public interface IAlertService
{
    Task DisplayAlert(string title, string message);
}