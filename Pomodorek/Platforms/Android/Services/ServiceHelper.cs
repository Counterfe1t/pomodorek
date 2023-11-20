namespace Pomodorek.Platforms.Android.Services;

public class ServiceHelper
{
    public static TService GetService<TService>() =>
        (TService)MauiApplication.Current.Services.GetService(typeof(TService));
}