namespace Pomodorek.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsPageViewModel _viewModel;

    public SettingsPage(SettingsPageViewModel viewModel)
    {
        InitializeComponent();
        InitializePage();

        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    public async Task<bool> CanNavigateFrom()
    {
        if (_viewModel.IsChangePending && !await _viewModel.DisplayUnsavedChangesDialog())
            return false;

        return true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel?.InitializeSettings();
    }

    private void InitializePage()
    {
        NavigationPage.SetHasNavigationBar(this, false);
        NavigationPage.SetHasBackButton(this, false);
    }
}