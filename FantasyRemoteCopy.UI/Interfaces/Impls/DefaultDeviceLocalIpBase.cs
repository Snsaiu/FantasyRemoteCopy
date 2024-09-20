using System.Net;
using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 苹果设备获得本机ip
/// </summary>
public abstract class DefaultDeviceLocalIpBase : IGetLocalIp
{
    public Task<string> GetLocalIpAsync()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            string localIp = ip.ToString();

            if (ip.AddressFamily == AddressFamily.InterNetwork && localIp.StartsWith("192.168"))
            {
                return Task.FromResult<string>(localIp);
            }
        }

        throw new NullReferenceException("无法找到本机ip");

    }
}