using FantasyRemoteCopy.Core.Models;

namespace FantasyRemoteCopy.Core.Consts;

public static class ConstParams
{
    /// <summary>
    /// 设备发现端口号
    /// </summary>
    public  static readonly string INVITE_PORT = "5976";

    /// <summary>
    /// upd建立tcpip端口号
    /// </summary>
    public static readonly string BuildTcpIp_Port = "5977";

    /// <summary>
    /// tpc接收端口号
    /// </summary>
    public static readonly string TcpIp_Port = "5978";


    /// <summary>
    /// 接收到的元数据
    /// </summary>
    public static List<DataMetaModel> ReceiveMetas=new List<DataMetaModel>();

    /// <summary>
    /// 即将发送的数据队列
    /// </summary>
    public static List<DataMetaModel> WillSendMetasQueue=new List<DataMetaModel>();


    /// <summary>
    /// 数据内容
    /// </summary>
    public static List<DataContent> DataContents= new List<DataContent>();
}