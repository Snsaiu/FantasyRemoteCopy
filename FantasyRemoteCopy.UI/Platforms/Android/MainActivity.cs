using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Text.Format;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Net;
using Environment = Android.OS.Environment;

namespace FantasyRemoteCopy.UI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
            ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this,
                new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 1);
        }
    }
}