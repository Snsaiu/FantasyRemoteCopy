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
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
			EndPoint endPoint = new IPEndPoint(IPAddress.Any, 5976);
			udpSocket.Bind(endPoint);
			Thread t = new Thread(() =>
			{

				while(true)
				{
					byte[] data = new byte[1024];
					int length=udpSocket.Receive(data);

				}

			});
			t.IsBackground = true;
			t.Start();

        }
	}
}

