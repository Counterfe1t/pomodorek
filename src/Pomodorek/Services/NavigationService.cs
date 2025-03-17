
namespace Pomodorek.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Application _application;

    public NavigationService(
        IServiceProvider serviceProvider,
        IApplicationService applicationService)
    {
        _serviceProvider = serviceProvider;
        _application = applicationService.Application;
    }

    public async Task NavigateToPageAsync<TPage>() where TPage : Page
    {
        if (_application?.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.PushAsync(_serviceProvider.GetRequiredService<TPage>());
        }
    }

    public async Task GoBackAsync()
    {
        if (_application?.MainPage is NavigationPage navigationPage)
        {
            await navigationPage.PopAsync();
        }
    }
}