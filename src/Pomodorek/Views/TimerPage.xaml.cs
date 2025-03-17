namespace Pomodorek.Views;

public partial class TimerPage : ContentPage
{
    public TimerPage(TimerPageViewModel viewModel)
    {
        InitializeComponent();
        InitializePage();

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not TimerPageViewModel viewModel)
            return;

        await viewModel.CheckAndRequestPermissionsAsync();

        // Update clock when the page is shown, unless the timer is running.
        if (viewModel.IsStopped)
            viewModel.UpdateClock();
    }

    private void InitializePage()
    {
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetHasBackButton(this, false);
    }
}