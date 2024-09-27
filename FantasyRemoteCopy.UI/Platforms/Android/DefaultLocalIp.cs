using Android.Content;
using Android.Net.Wifi;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using Formatter = Android.Text.Format.Formatter;


namespace FantasyRemoteCopy.UI
{
    public sealed class DefaultLocalIp : DeviceLocalIpBase
    {
        [Obsolete]
        public override Task<string> GetLocalIpAsync()
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
            string? ip = Formatter.FormatIpAddress(wm.ConnectionInfo.IpAddress);
            return ip is null ? throw new NullReferenceException("无法获得ip地址") : Task.FromResult(ip);
        }
    }
}