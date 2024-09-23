using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Consts;

public static class ConstParams
{
    /// <summary>
    /// 设备发现端口号
    /// </summary>
    public static readonly int INVITE_PORT = 5976;

    public static readonly int JOIN_PORT = 5977;

    public static readonly int TCP_PORT = 5978;

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
    public static List<DataMetaModel> ReceiveMetas = [];

    /// <summary>
    /// 即将发送的数据队列
    /// </summary>
    public static List<DataMetaModel> WillSendMetasQueue = [];


    /// <summary>
    /// 数据内容
    /// </summary>
    public static List<DataContent> DataContents = [];
}