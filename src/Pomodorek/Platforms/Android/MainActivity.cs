using Android.App;
using Android.Content.PM;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace Pomodorek;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity ActivityCurrent { get; set; }

    public MainActivity()
    {
        ActivityCurrent = this;
    }

    public override async void OnBackPressed()
    {
        var currentPage = (MauiApplication.Current.MainPage as NavigationPage)?.CurrentPage;

        if (currentPage is SettingsPage settingsPage && !await settingsPage.CanNavigateFrom())
            return;
        
        base.OnBackPressed();
    }
}