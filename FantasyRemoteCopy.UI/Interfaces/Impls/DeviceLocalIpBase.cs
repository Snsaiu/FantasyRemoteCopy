using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 设备获得本机ip
/// </summary>
public abstract class DeviceLocalIpBase : IGetLocalIp
{
    public Task<string> GetLocalIpAsync()
    {
        IEnumerable<NetworkInterface> networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.NetworkInterfaceType is NetworkInterfaceType.Wireless80211 or
                          NetworkInterfaceType.Ethernet);

        foreach (NetworkInterface? nic in networkInterfaces)
        {
            IPInterfaceProperties ipProps = nic.GetIPProperties();
            UnicastIPAddressInformation? ipAddress = ipProps.UnicastAddresses
                .FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);

            if (ipAddress != null)
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    return Task.FromResult(ipAddress.Address.ToString());
                }

            }
        }

        throw new NullReferenceException("无法找到本机ip");
    }
}