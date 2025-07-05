namespace Pomodorek.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableObject
{
    private string _title = string.Empty;
    private bool _isBusy = false;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            SetProperty(ref _isBusy, value);
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    public bool IsNotBusy => !IsBusy;

    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public ViewModelBase(string title, INavigationService navigationService)
    {
        Title = title;
        NavigationService = navigationService;

        InitializeAsyncCommand = new AsyncRelayCommand(
            async () => await IsBusyFor(InitializeAsync),
            AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    protected virtual Task InitializeAsync()
        => Task.CompletedTask;

    protected async Task IsBusyFor(Func<Task> unitOfWork)
    {
        IsBusy = true;

        try
        {
            await unitOfWork();
        }
        catch (Exception )
        {
            // TODO: Handle exceptions
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToTimerPageAsync()
        => await NavigationService.NavigateToAsync(AppResources.TimerPage_Route);

    [RelayCommand]
    private async Task NavigateToSettingsPageAsync()
        => await NavigationService.NavigateToAsync(AppResources.SettingsPage_Route);

    [RelayCommand]
    private async Task NavigateToAboutPageAsync()
        => await NavigationService.NavigateToAsync(AppResources.AboutPage_Route);
}