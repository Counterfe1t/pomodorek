namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ImageButtonControl
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(ImageButtonControl),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((ImageButtonControl)bindable).ImageButton.Source = newValue as string;
        });

    public string Source
    {
        get => GetValue(SourceProperty) as string;
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty ImageButtonCommandProperty = BindableProperty.Create(
        nameof(ImageButtonCommand),
        typeof(RelayCommand),
        typeof(ImageButtonControl),
        default(RelayCommand),
        BindingMode.OneWay);

    public RelayCommand ImageButtonCommand
    {
        get => GetValue(ImageButtonCommandProperty) as RelayCommand;
        set => SetValue(ImageButtonCommandProperty, value);
    }

    public ImageButtonControl()
    {
        InitializeComponent();
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        ImageButton.Source = Source;
        ImageButton.Command = new Command(() => ImageButtonCommand.Execute(null));
    }
}