using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using Pomodorek.ViewModels;
using Pomodorek.Views;
using System.Reflection;
using IPomodorekNotificationService = Pomodorek.Services.INotificationService;

namespace Pomodorek;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() =>
        MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
#if ANDROID
            .UseLocalNotification()
#endif
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

    #region Services

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
#if WINDOWS || ANDROID
        builder.Services.AddSingleton(FileSystem.Current);
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddSingleton<IPomodorekNotificationService, NotificationService>();
        builder.Services.AddSingleton<ITimerService, TimerService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
        builder.Services.AddSingleton<ISoundService, SoundService>();
#endif
#if ANDROID
        builder.Services.AddSingleton<IForegroundService, ForegroundService>();
#endif
        return builder;
    }

    #endregion

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<SettingsPageViewModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
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