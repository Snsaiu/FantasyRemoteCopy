using System;
using System.Net;
using System.Net.Sockets;
using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls
{
	public class DefaultLocalIp:IGetLocalIp
	{
		public DefaultLocalIp()
		{
		}

		public ResultBase<string> GetLocalIp()
		{
			try
			{
				IPHostEntry host;
				string localIP = "";
				host = Dns.GetHostEntry(Dns.GetHostName());

				foreach (IPAddress ip in host.AddressList)
				{
					localIP = ip.ToString();

					string[] temp = localIP.Split('.');

					if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
					{
						break;
					}
					else
					{
						localIP = null;
					}
				}

				if (string.IsNullOrEmpty(localIP))
					return new ErrorResultModel<string>("无法获得本级ip地址");

				return new SuccessResultModel<string>(localIP);
			}
			catch (Exception e)
			{
				return new ErrorResultModel<string>(e.Message);
			}
			
		}
	}
}

