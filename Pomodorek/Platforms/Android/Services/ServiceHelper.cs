namespace Pomodorek.Platforms.Android.Services;

public class ServiceHelper
{
    public static TService GetService<TService>() =>
        (TService)IPlatformApplication.Current.Services.GetService(typeof(TService));
}