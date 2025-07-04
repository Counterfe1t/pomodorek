namespace Pomodorek.Views.Base;

public abstract class ContentPageBase : ContentPage
{
    protected ContentPageBase()
    {
        NavigationPage.SetBackButtonTitle(this, string.Empty);
    }

    // TODO: Execute initialization logic here
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}