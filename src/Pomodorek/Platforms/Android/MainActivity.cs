﻿using Android.App;
using Android.Content.PM;

namespace Pomodorek;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity? ActivityCurrent { get; set; }

    public MainActivity()
    {
        ActivityCurrent = this;
    }
}