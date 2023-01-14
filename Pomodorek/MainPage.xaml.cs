using Pomodorek.ViewModels;

namespace Pomodorek;

public partial class MainPage : ContentPage
{
    public MainPageViewModel ViewModel { get; set; }

    public MainPage()
	{
        // todo: inject MainPageViewModel
		InitializeComponent();
        ViewModel = new MainPageViewModel();
        BindingContext = ViewModel;
    }

    #region Events

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        ViewModel.StartSession();
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        ViewModel.StopSession();
    }

    #endregion
}

