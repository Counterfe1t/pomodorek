namespace Pomodorek.Views.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NavbarItem
{
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(NavbarItem),
        default(string),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((NavbarItem)bindable).ImageButton.Source = newValue as string;
        });

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(AsyncRelayCommand),
        typeof(NavbarItem),
        default(AsyncRelayCommand),
        BindingMode.OneWay);

    public AsyncRelayCommand Command
    {
        get => (AsyncRelayCommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(NavbarItem),
        default(bool),
        BindingMode.OneWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var isSelected = (bool)newValue;
            var appTheme = Application.Current?.RequestedTheme ?? AppTheme.Light;

            if (isSelected)
                ((NavbarItem)bindable).ImageButton.SetAppThemeColor(
                    BackgroundColorProperty,
                    Color.FromArgb("#E1E1E1"),
                    Color.FromArgb("#212121"));
        });

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public NavbarItem()
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