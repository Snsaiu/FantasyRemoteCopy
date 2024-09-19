using Android.Content;
using Android.Net.Wifi;

using FantasyRemoteCopy.UI.Interfaces;

using FantasyResultModel;
using FantasyResultModel.Impls;

using Formatter = Android.Text.Format.Formatter;

//using Application = Microsoft.Maui.Controls.PlatformConfiguration.Android.App;


namespace FantasyRemoteCopy.UI
{
    public class DefaultLocalIp : IGetLocalIp
    {

        [Obsolete]
        public Task<string> GetLocalIpAsync()
        {
            
                Context context = Android.App.Application.Context;
                if (context.GetSystemService(Context.WifiService) is not WifiManager wm)
                {
                   throw new NullReferenceException("获得wifi管理器失败");
                }
                if (wm.ConnectionInfo is null)
                {
                    throw new NullReferenceException("获得wifi信息失败");
                }
                var ip = Formatter.FormatIpAddress(wm.ConnectionInfo.IpAddress);
                if (ip is null)
                    throw new NullReferenceException("无法获得ip地址");
                return Task.FromResult(ip);
        }
    }
}

