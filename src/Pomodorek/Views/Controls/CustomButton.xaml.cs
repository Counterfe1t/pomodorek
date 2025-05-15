namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomButton
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(CustomButton),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((CustomButton)bindable).Button.Text = newValue as string;
        });

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(RelayCommand),
        typeof(CustomButton),
        default(RelayCommand),
        BindingMode.OneWay);

    public RelayCommand Command
    {
        get => (RelayCommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public CustomButton()
    {
        InitializeComponent();
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        Button.Text = Text;
        Button.Command = new Command(() => Command.Execute(null));
    }
}