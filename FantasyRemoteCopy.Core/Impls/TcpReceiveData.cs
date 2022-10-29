using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;


using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls
{
    public class TcpReceiveData : IReceiveData
    {
        public TcpReceiveData()
        {

        }

        public event ReceiveDataDelegate ReceiveDataEvent;
        public event ReceiveInviteDelegate ReceiveInviteEvent;
        public event ReceiveBuildConnectionDelegate ReceiveBuildConnectionEvent;
        public event ReceivingDataDelegate ReceivingDataEvent;



       


        /// <summary>
        /// 建立tcp数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="byteCount"></param>
        public  void LiseningData(string ip, long byteCount)
        {

           var listener = new TcpListener(IPAddress.Parse(ip), int.Parse(ConstParams.TcpIp_Port));
           listener.Start();
            Task.Run(() =>
           {

               while (true)
               {
                   try
                   {
                       TcpClient client = listener.AcceptTcpClient();

                       if (client.Connected)
                       {
                          
                       }
                       NetworkStream stream = client.GetStream();
                       if (stream != null)
                       {
                           byte[] buffer = new byte[byteCount+1024*1024];
                               int real_count= stream.Read(buffer,0,buffer.Length);
                             
                               stream.Flush();
                               stream.Close();
                               client.Close();

                           TransformData td =
                               JsonConvert.DeserializeObject<TransformData>(Encoding.UTF8.GetString(buffer, 0, real_count));

                           this.ReceiveDataEvent?.Invoke(td);

                       }
                       return;
                   }
                   catch (Exception ex)
                   {
                     
                   }
               }



           }).GetAwaiter().OnCompleted(() =>
           {

               listener.Stop();

           });


        
   
            //Socket tcpSocket = null;
            //Socket socket = null;
            //try
            //{
            //    tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    tcpSocket.Bind(new IPEndPoint(IPAddress.Any, int.Parse(ConstParams.TcpIp_Port)));
            //    this.ReceivingDataEvent?.Invoke(ip);
            //    tcpSocket.Listen();

            //    byte[] bytes = new byte[byteCount + 2097152];
            //    socket = await tcpSocket.AcceptAsync();
            //    int length = await socket.ReceiveAsync(bytes, SocketFlags.None);
            //    TransformData td =
            //        JsonConvert.DeserializeObject<TransformData>(Encoding.UTF8.GetString(bytes, 0, length));

            //    this.ReceiveDataEvent?.Invoke(td);
            //}
            //catch (Exception e)
            //{

            //    throw;
            //}
            //finally
            //{
            //    socket.Close();
            //    socket = null;
            //    tcpSocket.Close();
            //    tcpSocket = null;
            //}
        }


        /// <summary>
        /// 建立邀请
        /// </summary>
        public void LiseningInvite()
        {
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, int.Parse(ConstParams.INVITE_PORT));
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
                    DataMetaModel dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(transformdata.Data));
                    ConstParams.ReceiveMetas.Add(dmm);

                    transformdata.TargetIp = (((System.Net.IPEndPoint)res.RemoteEndPoint).Address).ToString();

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

