using CommunityToolkit.Maui;
using Plugin.LocalNotification;
using Pomodorek.Pages;
using Pomodorek.Services;
using Pomodorek.ViewModels;
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
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
#if WINDOWS || ANDROID
        builder.Services.AddSingleton<IPomodorekNotificationService, NotificationService>();
        builder.Services.AddSingleton<ITimerService, TimerService>();
#endif
#if ANDROID
        builder.Services.AddSingleton<IForegroundService, ForegroundService>();
#endif
        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPageViewModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
        return builder;
    }
}