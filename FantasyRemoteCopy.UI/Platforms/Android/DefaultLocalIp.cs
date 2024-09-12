﻿using Android.Content;
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
        public DefaultLocalIp()
        {
        }

        [Obsolete]
        public ResultBase<List<string>> GetLocalIp()
        {
            try
            {
                List<string> ips = [];

                Context context = Android.App.Application.Context;
                if (context.GetSystemService(Context.WifiService) is not WifiManager wm)
                {
                    return new ErrorResultModel<List<string>>("获得wifi管理器失败");
                }
                if (wm.ConnectionInfo is null)
                {
                    return new ErrorResultModel<List<string>>("获得wifi信息失败");
                }
                string? ip = Formatter.FormatIpAddress(wm.ConnectionInfo.IpAddress);



                return ips.Count == 0 ? new ErrorResultModel<List<string>>("无法获得本机ip地址") : new SuccessResultModel<List<string>>(ips);
            }
            catch (Exception e)
            {
                return new ErrorResultModel<List<string>>(e.Message);
            }

        }
    }
}

