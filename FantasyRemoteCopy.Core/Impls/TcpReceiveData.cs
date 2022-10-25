using System;
using System.Net;
using System.Net.Sockets;

namespace FantasyRemoteCopy.Core.Impls
{
	public class TcpReceiveData:IReceiveData
	{
		public TcpReceiveData()
		{

		}

		public void LiseningData()
		{

		}

		public void LiseningInvite()
		{
            var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ipaddress = IPAddress.Parse("0.0.0.0");
			EndPoint endPoint = new IPEndPoint(IPAddress.Any, 7090);
			tcpClient.Connect(endPoint);
			Thread t = new Thread(() =>
			{

				while(true)
				{
					byte[] data = new byte[1024];
					int length=tcpClient.Receive(data);

				}

			});
			t.Start();

        }
	}
}

