using System;
using System.Net;
using System.Net.Sockets;
using FantasyResultModel;
using FantasyResultModel.Impls;

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
				IPHostEntry host;
				string localIP = "";
				host = Dns.GetHostEntry(Dns.GetHostName());

				foreach (IPAddress ip in host.AddressList)
				{
					localIP = ip.ToString();

					string[] temp = localIP.Split('.');

					if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                    {
                        ips.Add(localIP);
                    }
					else
					{
						localIP = null;
					}
				}

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

