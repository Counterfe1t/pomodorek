namespace Pomodorek.Interfaces;

public interface INavigationService
{
    /// <summary>
    /// Navigate to the page at specified route.
    /// </summary>
    /// <param name="route">The unique identifier or URI of the target page.</param>
    Task NavigateToAsync(string route);
}