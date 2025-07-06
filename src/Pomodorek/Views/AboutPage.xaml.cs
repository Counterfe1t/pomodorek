namespace Pomodorek.Views;

public partial class AboutPage : ContentPageBase
{
    public AboutPage(AboutPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}