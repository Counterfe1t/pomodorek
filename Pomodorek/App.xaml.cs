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

        if (DeviceInfo.Platform == DevicePlatform.WinUI ||
            DeviceInfo.Platform == DevicePlatform.MacCatalyst)
        {
            window.Width = 800;
            window.MinimumWidth = 800;
            window.MaximumWidth = 1920;
            window.Height= 600;
            window.MinimumHeight = 600;
            window.MaximumHeight = 1080;
        }

        return window;
    }
}
