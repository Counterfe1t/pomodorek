namespace Pomodorek;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() =>
        MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windows => windows.OnWindowCreated(window =>
                {
                    WinUI.Program.CurrentWindow = window as MauiWinUIWindow;
                }));
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterConfiguration()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
#if WINDOWS || ANDROID
        builder.Services.AddSingleton(FileSystem.Current);
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddSingleton(WeakReferenceMessenger.Default);
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton<ITimerService, TimerService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
        builder.Services.AddSingleton<ISoundService, SoundService>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        builder.Services.AddSingleton<IPermissionsService, PermissionsService>();
        builder.Services.AddSingleton<ISessionService, SessionService>();
#endif
        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<TimerPageViewModel>();
        builder.Services.AddSingleton<SettingsPageViewModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<TimerPage>();
        builder.Services.AddSingleton<SettingsPage>();
        return builder;
    }

    public static MauiAppBuilder RegisterConfiguration(this MauiAppBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(Constants.AppSettingsFileName);

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);
        return builder;
    }
}