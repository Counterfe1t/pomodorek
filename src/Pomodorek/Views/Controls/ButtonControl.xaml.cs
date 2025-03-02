using System.Windows.Input;

namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ButtonControl
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ButtonControl),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((ButtonControl)bindable).Button.Text = newValue as string;
        });

    public string Text
    {
        get => GetValue(TextProperty) as string;
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(
        nameof(ButtonCommand),
        typeof(ICommand),
        typeof(ButtonControl),
        default(ICommand),
        BindingMode.OneWay);

    public ICommand ButtonCommand
    {
        get => GetValue(ButtonCommandProperty) as ICommand;
        set => SetValue(ButtonCommandProperty, value);
    }

    public ButtonControl()
    {
        InitializeComponent();
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        Button.Text = Text;
        Button.Command = new Command(() => ButtonCommand.Execute(null));
    }
}