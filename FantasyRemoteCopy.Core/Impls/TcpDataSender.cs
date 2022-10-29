using System.Net;
using System.Net.Sockets;
using System.Text;

using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls;

public class TcpDataSender:ISendData
{
    
    public TcpDataSender()
    {
 
        
    }


    /// <summary>
    /// 向远程主机发送数据
    /// </summary>
    /// <param name="socket">要发送数据且已经连接到远程主机的 Socket</param>
    /// <param name="buffer">待发送的数据</param>
    /// <param name="outTime">发送数据的超时时间，以秒为单位，可以精确到微秒</param>
    /// <returns>0:发送数据成功；-1:超时；-2:发送数据出现错误；-3:发送数据时出现异常</returns>
    /// <remarks >
    /// 当 outTime 指定为-1时，将一直等待直到有数据需要发送
    /// </remarks>
    private  int sendData(Socket socket, byte[] buffer, int outTime)
    {
        if (socket == null || socket.Connected == false)
        {
            throw new ArgumentException("参数socket 为null，或者未连接到远程计算机");
        }
        if (buffer == null || buffer.Length == 0)
        {
            throw new ArgumentException("参数buffer 为null ,或者长度为 0");
        }

        int flag = 0;
        try
        {
            int left = buffer.Length;
            int sndLen = 0;

            while (true)
            {
                if ((socket.Poll(outTime * 100, SelectMode.SelectWrite) == true))
                {        // 收集了足够多的传出数据后开始发送
                    sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                    left -= sndLen;
                    if (left == 0)
                    {                                        // 数据已经全部发送
                        flag = 0;
                        break;
                    }
                    else
                    {
                        if (sndLen > 0)
                        {                                    // 数据部分已经被发送
                            continue;
                        }
                        else
                        {                                                // 发送数据发生错误
                            flag = -2;
                            break;
                        }
                    }
                }
                else
                {                                                        // 超时退出
                    flag = -1;
                    break;
                }
            }
        }
        catch (SocketException e)
        {

            flag = -3;
        }
        return flag;
    }

    public async Task<ResultBase<bool>> SendRquestBuildConnectionDataAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        var s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendInviteAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress,int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.PacketInformation,true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        var s = new ArraySegment<byte>(byteData);
       int res=  await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendBuildConnectionAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);

        string dataStr = JsonConvert.SerializeObject(data);
        byte[] byteData = Encoding.UTF8.GetBytes(dataStr);
        var s = new ArraySegment<byte>(byteData);
        int res = await udpClient.SendToAsync(s, SocketFlags.None, point);
        udpClient.Close();
        return null;
    }

    public async Task<ResultBase<bool>> SendDataAsync(DataMetaModel data,string content,string deviceNickName)
    {
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress, int.Parse(ConstParams.TcpIp_Port));
        
        //only for text
        if(data.DataType==DataType.Text)
        {
            tcpClient.Connect(point);
            TransformData td = new TransformData();
            td.TargetDeviceNickName= deviceNickName;
            td.DataGuid = data.Guid ;
            td.TargetIp = data.TargetIp;
            td.Port = ConstParams.TcpIp_Port;
           
            td.Type = Enums.TransformType.SendingTxtData;

            td.Data= Encoding.UTF8.GetBytes(content);

            ArraySegment<byte>b= Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));
           
            this.sendData(tcpClient, b.ToArray(),10);
            // await tcpClient.SendAsync(b, SocketFlags.None);
        
           var dmm= ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if(dmm!=null)
            {
                dmm.State = MetaState.Sended;
            }
           
            tcpClient.Close();
         
        }
        else // for file type
        {
            tcpClient.Connect(point);
            TransformData td = new TransformData();
            td.TargetDeviceNickName = deviceNickName;
            td.DataGuid = data.Guid;
            td.TargetIp = data.TargetIp;
            td.Port = ConstParams.TcpIp_Port;

            td.Type = Enums.TransformType.SendingFileData;
            
            //todo td.data is filename or file byte[]

            FileDataModel fdm=new FileDataModel();
            fdm.FileNameWithExtension = Path.GetFileName( content);

            var dmm = ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if (dmm == null)
            {
                return new ErrorResultModel<bool>("队列中没有发现文件信息");
            }

            FileStream st = new FileStream(content, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            byte[] contentBytes = new byte[st.Length];

            int readAsync = await st.ReadAsync(contentBytes,0,(int)st.Length);

            st.Close();
            fdm.ContentBytes = contentBytes.ToArray();

            td.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fdm));

            ArraySegment<byte> b = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));
            await tcpClient.SendAsync(b, SocketFlags.None);

            dmm.State = MetaState.Sended;

            tcpClient.Close();
        }
        return new SuccessResultModel<bool>(true);
    }


}