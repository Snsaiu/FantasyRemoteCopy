using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
			udpSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.PacketInformation,true);
			udpSocket.Bind(endPoint);

			_ = Task.Run(async () =>
			{

				SocketReceiveFromResult res;
				byte[] _buffer_recv = new byte[4096];
				ArraySegment<byte> _buffer_recv_segment = new(_buffer_recv);
				
				while (true)
				{

					res = await udpSocket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, endPoint);
					Console.WriteLine($"接收消息{Encoding.UTF8.GetString(_buffer_recv,0,res.ReceivedBytes)}");
				}
			});


		}
	}
}

