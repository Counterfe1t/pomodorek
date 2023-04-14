namespace Pomodorek.Views;

public partial class SettingsPage : ContentPage
{
	private readonly SettingsPageViewModel _viewModel;

	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel?.InitializeSettings();
    }
}