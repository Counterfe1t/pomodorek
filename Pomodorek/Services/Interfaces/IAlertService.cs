namespace Pomodorek.Services;

public interface IAlertService
{
    Task DisplayAlertAsync(string title, string message);
}