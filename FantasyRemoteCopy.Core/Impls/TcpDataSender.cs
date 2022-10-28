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
            if(data.DataType== DataType.Text)
            {
                td.Type = Enums.TransformType.SendingTxtData;
            }
            else
            {
                td.Type = Enums.TransformType.SendingFileData;
            }
          
            td.Data= Encoding.UTF8.GetBytes(content);

            ArraySegment<byte>b= Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(td));
            await tcpClient.SendAsync(b, SocketFlags.None);
        
           var dmm= ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == data.Guid);
            if(dmm!=null)
            {
                dmm.State = MetaState.Sended;
            }
           
            tcpClient.Close();
         
        }
        else // for file type
        {

        }
        return new SuccessResultModel<bool>(true);
    }


}