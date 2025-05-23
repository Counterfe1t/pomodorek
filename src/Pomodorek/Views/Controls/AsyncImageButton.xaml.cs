namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AsyncImageButton
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(AsyncImageButton),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((AsyncImageButton)bindable).ImageButton.Source = newValue as string;
        });

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(AsyncRelayCommand),
        typeof(AsyncImageButton),
        default(AsyncRelayCommand),
        BindingMode.OneWay);

    public AsyncRelayCommand Command
    {
        get => (AsyncRelayCommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public AsyncImageButton()
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