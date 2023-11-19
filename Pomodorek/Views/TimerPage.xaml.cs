namespace Pomodorek.Views;

public partial class TimerPage : ContentPage
{
    public TimerPage(TimerPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as TimerPageViewModel;

        await viewModel.CheckAndRequestPermissionsAsync();

        if (!viewModel.IsRunning)
        {
            viewModel.UpdateTimerUI();
        }
    }
}