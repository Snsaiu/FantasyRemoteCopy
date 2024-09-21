using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Models;

public class LocalNetDeviceDiscoveryReceiveMessage :IDeviceName
{
    [JsonConstructor]
    public LocalNetDeviceDiscoveryReceiveMessage()
    {
        
    }
    
    public LocalNetDeviceDiscoveryReceiveMessage(string name,string ip)
    {
        Name = name;
        Ip = ip;
    }

    public string Name { get; }

    public string Ip { get; }
}