namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AsyncImageButtonControl
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(AsyncImageButtonControl),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((AsyncImageButtonControl)bindable).ImageButton.Source = newValue as string;
        });

    public string Source
    {
        get => GetValue(SourceProperty) as string;
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(AsyncRelayCommand),
        typeof(AsyncImageButtonControl),
        default(AsyncRelayCommand),
        BindingMode.OneWay);

    public AsyncRelayCommand Command
    {
        get => GetValue(CommandProperty) as AsyncRelayCommand;
        set => SetValue(CommandProperty, value);
    }

    public AsyncImageButtonControl()
    {
        InitializeComponent();
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        ImageButton.Source = Source;
        ImageButton.Command = new AsyncRelayCommand(async () => await Command.ExecuteAsync(null));
    }
}