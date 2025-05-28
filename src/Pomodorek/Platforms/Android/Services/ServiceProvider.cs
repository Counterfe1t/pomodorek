namespace Pomodorek.Platforms.Android.Services;

public class ServiceProvider
{
    public static TService GetService<TService>()
        => (TService?)IPlatformApplication.Current?.Services.GetService(typeof(TService))
        ?? throw new Exception($"Could not instantiate servoce of type {nameof(TService)}");
}