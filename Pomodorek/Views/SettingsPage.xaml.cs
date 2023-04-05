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

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _viewModel.InitializeCommand.Execute(null);
    }
}