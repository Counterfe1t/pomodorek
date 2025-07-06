namespace Pomodorek.Views;

public partial class TimerPage : ContentPageBase
{
    public TimerPage(TimerPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}