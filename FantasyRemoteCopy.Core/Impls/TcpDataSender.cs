using System.Net;
using System.Net.Sockets;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls;

public class TcpDataSender:ISendData
{
    
    public TcpDataSender()
    {
 
        
    }

    public async Task<ResultBase<bool>> SendDataAsync(TransformData data)
    {
        var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress,int.Parse(data.Port));
        tcpClient.Connect(point); 
        var res= await tcpClient.SendAsync(data.Data, SocketFlags.None);
        return await Task.FromResult(new SuccessResultModel<bool>(true));
    }

    public async Task<ResultBase<bool>> SendInviteAsync(TransformData data)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress,int.Parse(data.Port));

        udpClient.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.PacketInformation,true);
        var s = new ArraySegment<byte>(data.Data);
        await udpClient.SendToAsync(s, SocketFlags.None, point);
        
        return null;
    }
}