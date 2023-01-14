namespace Pomodorek;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

#if WINDOWS
        window.Width = 800;
        window.Height = 600;
#endif

        return window;
    }
}
