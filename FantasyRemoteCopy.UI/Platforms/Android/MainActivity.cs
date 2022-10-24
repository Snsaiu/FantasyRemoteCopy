using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Text.Format;

namespace FantasyRemoteCopy.UI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
       
        Context context = this.Application.ApplicationContext;
        WifiManager wm = (WifiManager)context.GetSystemService(Context.WifiService);
        String ip = Formatter.FormatIpAddress(wm.ConnectionInfo.IpAddress);

        base.OnCreate(savedInstanceState);
    }
}

