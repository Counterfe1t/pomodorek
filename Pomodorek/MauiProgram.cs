using CommunityToolkit.Maui;
using Pomodorek.Services;
using Pomodorek.ViewModels;
using Pomodorek.Views;

namespace Pomodorek;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

#if WINDOWS || ANDROID
        builder.Services.AddSingleton<INotificationService, NotificationService>();
#endif
        return builder.Build();
    }
}