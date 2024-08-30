﻿using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;

using System.Net;
using System.Net.Sockets;
using System.Text;

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
        public event ReceivedFileFinishedDelegate ReceivedFileFinishedEvent;
        public event ReceivingProcessDelegate ReceivingProcessEvent;


        /// <summary>
        /// 建立tcp数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="byteCount"></param>
        [Obsolete]
        public void LiseningData(string ip, long byteCount)
        {

            TcpListener listener = new TcpListener(int.Parse(ConstParams.TcpIp_Port));
            listener.Start();
            Task.Run(() =>
           {
               ReceivingDataEvent?.Invoke(ip);
               while (true)
               {
                   try
                   {
                       TcpClient client = listener.AcceptTcpClient();

                       NetworkStream stream = client.GetStream();
                       if (stream != null)
                       {
                           byte[] buffer = new byte[1024];

                           long currentBytes = 0;
                           int bytesRead = 0;          // 读取的字节数

                           MemoryStream msStream = new MemoryStream();
                           bool header = true;
                           do
                           {
                               bytesRead = stream.Read(buffer, 0, 1024);
                               if (header)
                               {
                                   string size = Encoding.UTF8.GetString(buffer);
                                   byteCount = long.Parse(size);
                                   header = false;
                                   continue;
                               }
                               msStream.Write(buffer, 0, bytesRead);
                               currentBytes += bytesRead;
                               double currentProcess = Math.Round(currentBytes / (double)byteCount, 2);
                               ReceivingProcessEvent?.Invoke(ip, currentProcess);


                           } while (bytesRead > 0);

                           byte[] resbuffer = msStream.GetBuffer();
                           // string msg = Encoding.UTF8.GetString(buffer);

                           TransformData td =
                               JsonConvert.DeserializeObject<TransformData>(Encoding.UTF8.GetString(resbuffer));
                           ReceivedFileFinishedEvent?.Invoke(ip);

                           ReceiveDataEvent?.Invoke(td);

                       }
                       return;
                   }
                   catch (Exception)
                   {

                   }
               }
           }).GetAwaiter().OnCompleted(() =>
           {
               listener.Stop();

           });
        }


        /// <summary>
        /// 建立邀请
        /// </summary>
        public void LiseningInvite()
        {
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, int.Parse(ConstParams.INVITE_PORT));
            udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            udpSocket.Bind(endPoint);
            Task.Run(async () =>
             {
                 SocketReceiveFromResult res;
                 byte[] _buffer_recv = new byte[4096];
                 ArraySegment<byte> _buffer_recv_segment = new(_buffer_recv);

                 while (true)
                 {
                     res = await udpSocket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, endPoint);
                     string str = Encoding.UTF8.GetString(_buffer_recv, 0, res.ReceivedBytes);

                     TransformData transformdata = JsonConvert.DeserializeObject<TransformData>(str);
                     transformdata.TargetIp = (((System.Net.IPEndPoint)res.RemoteEndPoint).Address).ToString();

                     // transformdata.TargetIp;
                     ReceiveInviteEvent?.Invoke(transformdata);

                 }
             });


        }





        public void LiseningBuildConnection()
        {
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

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

                    TransformData transformdata = JsonConvert.DeserializeObject<TransformData>(str);
                    DataMetaModel dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(transformdata.Data));
                    ConstParams.ReceiveMetas.Add(dmm);

                    transformdata.TargetIp = (((System.Net.IPEndPoint)res.RemoteEndPoint).Address).ToString();

                    ReceiveBuildConnectionEvent?.Invoke(transformdata);

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

