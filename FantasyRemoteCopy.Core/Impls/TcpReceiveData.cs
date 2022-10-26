using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls
{
	public class TcpReceiveData:IReceiveData
	{
		public TcpReceiveData()
		{

		}

		public event ReceiveDataDelegate ReceiveDataEvent;
		public event ReceiveInviteDelegate ReceiveInviteEvent;
        public event ReceiveBuildConnectionDelegate ReceiveBuildConnectionEvent;

        /// <summary>
        /// 建立tcp数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="byteCount"></param>
		public void LiseningData(string ip,long byteCount)
		{
			var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), int.Parse(ConstParams.TcpIp_Port)));
            byte[] bytes = new byte[byteCount];
            int length= tcpSocket.Receive(bytes);
            TransformData td=JsonConvert.DeserializeObject<TransformData>(Encoding.UTF8.GetString(bytes));
            tcpSocket.Close();
        }


        public void LiseningInvite()
		{
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
			EndPoint endPoint = new IPEndPoint(IPAddress.Any, int.Parse( ConstParams.INVITE_PORT));
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
					string str = Encoding.UTF8.GetString(_buffer_recv, 0, res.ReceivedBytes);

					 var transformdata= JsonConvert.DeserializeObject<TransformData>(str);
					transformdata.TargetIp = (((System.Net.IPEndPoint)res.RemoteEndPoint).Address).ToString();
					
                    // transformdata.TargetIp;
                     this.ReceiveInviteEvent?.Invoke(transformdata);

				}
			});


		}


		


        public void LiseningBuildConnection()
        {
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, int.Parse(ConstParams.BuildTcpIp_Port));
            udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            udpSocket.Bind(endPoint);

            _ = Task.Run(async () =>
            {
                SocketReceiveFromResult res;
                byte[] _buffer_recv = new byte[4096];
                ArraySegment<byte> _buffer_recv_segment = new(_buffer_recv);

                while (true)
                {
                    res = await udpSocket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, endPoint);
                    string str = Encoding.UTF8.GetString(_buffer_recv, 0, res.ReceivedBytes);

                    var transformdata = JsonConvert.DeserializeObject<TransformData>(str);
                    DataMetaModel dmm=JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(transformdata.Data));
                    ConstParams.ReceiveMetas.Add(dmm);
                 
                    //  transformdata.TargetIp = (((System.Net.IPEndPoint)res.RemoteEndPoint).Address).ToString();

                    this.ReceiveBuildConnectionEvent?.Invoke(transformdata);

                    //if(transformdata.Type==Enums.DataType.RequestBuildConnect)
                    //            {

                    //                await  Task.Run(() =>
                    //                {

                    //                    this.LiseningData(transformdata.TargetIp, dmm.Size);

                    //                });

                    //            }



                }
            });
        }
    }
}

