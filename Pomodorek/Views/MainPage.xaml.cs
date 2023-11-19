namespace Pomodorek.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MainPageViewModel;

        await viewModel.CheckAndRequestPermissionsAsync();

        if (!viewModel.IsRunning)
        {
            viewModel.Initialize();
        }
    }
}