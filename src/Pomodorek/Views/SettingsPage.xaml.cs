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
        Shell.Current.Navigating += OnNavigating;
        _viewModel?.InitializeSettings();
    }

    protected override void OnDisappearing()
    {
        Shell.Current.Navigating -= OnNavigating;
        base.OnDisappearing();
    }

    private async void OnNavigating(object sender, ShellNavigatingEventArgs args)
    {
        // Dismiss navigation when navigating to current page.
        if (args.Target.Location.OriginalString.Contains(Constants.Pages.Settings))
            return;

        // Allow navigation when there are no unsaved changes.
        if (!_viewModel.IsChangePending)
            return;

        // Pause navigation.
        var deferral = args.GetDeferral();

        // Prompt user with dialog warning about unsaved changes.
        if (!await _viewModel.DisplayUnsavedChangesDialog())
            // Cancel navigation if user dismisses the dialog.
            args.Cancel();

        // Unpause navigation.
        deferral.Complete();
    }
}