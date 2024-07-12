namespace Pomodorek.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy = false;
    
    [ObservableProperty]
    private string _title;

    public BaseViewModel(string title)
    {
        Title = title;
    }
}