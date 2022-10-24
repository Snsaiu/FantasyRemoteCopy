using System;
using System.Net;
using System.Net.Sockets;
using Android.Content;
using Android.Net.Wifi;
using Android.Text.Format;
using FantasyResultModel;
using FantasyResultModel.Impls;
using Application=Android.App;


namespace FantasyRemoteCopy.Core.Platforms
{
	public class DefaultLocalIp:IGetLocalIp
	{
		public DefaultLocalIp()
		{
		}

		public ResultBase<List<string>> GetLocalIp()
		{
			try
			{
				List<string> ips=new List<string>();

                Context context = Application.Application.Context; //Android.ApplicationContext;
                WifiManager wm = (WifiManager)context.GetSystemService(Context.WifiService);
                String ip = Formatter.FormatIpAddress(wm.ConnectionInfo.IpAddress);



                if (ips.Count==0)
					return new ErrorResultModel<List<string>>("无法获得本机ip地址");

				return new SuccessResultModel<List<string>>(ips);
			}
			catch (Exception e)
			{
				return new ErrorResultModel<List<string>>(e.Message);
			}
			
		}
	}
}

