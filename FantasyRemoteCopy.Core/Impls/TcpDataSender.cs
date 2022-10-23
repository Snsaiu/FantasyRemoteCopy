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

    public async Task<ResultBase<bool>> SendAsync(TransformData data)
    {
        var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse(data.TargetIp);
        EndPoint point = new IPEndPoint(ipaddress,int.Parse(data.Port));
        tcpClient.Connect(point); 
        var res= await tcpClient.SendAsync(data.Data, SocketFlags.None);
        return await Task.FromResult(new SuccessResultModel<bool>(true));
    }
}