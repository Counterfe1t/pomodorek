namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomImageButton
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(CustomImageButton),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((CustomImageButton)bindable).ImageButton.Source = newValue as string;
        });

    public string Source
    {
        get => GetValue(SourceProperty) as string;
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(RelayCommand),
        typeof(CustomImageButton),
        default(RelayCommand),
        BindingMode.OneWay);

    public RelayCommand Command
    {
        get => GetValue(CommandProperty) as RelayCommand;
        set => SetValue(CommandProperty, value);
    }

    public CustomImageButton()
    {
        InitializeComponent();
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        ImageButton.Source = Source;
        ImageButton.Command = new RelayCommand(() => Command.Execute(null));
    }
}