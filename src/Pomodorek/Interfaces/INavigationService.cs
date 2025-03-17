namespace Pomodorek.Interfaces;

public interface INavigationService
{
    /// <summary>
    /// Navigates to the specified <see cref="Page"/>.
    /// </summary>
    /// <typeparam name="TPage">Destination <see cref="Page"/>.</typeparam>
    Task NavigateToPageAsync<TPage>() where TPage : Page;

    /// <summary>
    /// Navigates to the previous <see cref="Page"/>.
    /// </summary>
    Task GoBackAsync();
}