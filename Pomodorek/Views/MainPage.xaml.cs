using Pomodorek.ViewModels;

namespace Pomodorek.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    #region Events

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        _viewModel.StartSession();
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        _viewModel.StopSession();
    }

    #endregion
}

