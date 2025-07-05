namespace Pomodorek.Views.Base;

public abstract class ContentPageBase : ContentPage
{
    protected ContentPageBase()
    {
        NavigationPage.SetBackButtonTitle(this, string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not ViewModelBase viewModel)
            return;

        await viewModel.InitializeAsyncCommand.ExecuteAsync(null);
    }
}