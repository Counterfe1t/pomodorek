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

        if (BindingContext is not TimerPageViewModel viewModel)
            return;

        await viewModel.CheckAndRequestPermissionsAsync();

        if (viewModel.IsStopped)
            viewModel.UpdateClock();
    }
}