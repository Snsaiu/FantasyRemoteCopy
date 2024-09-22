using FantasyRemoteCopy.UI.Interfaces;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Models;

public class DeviceDiscoveryMessage :IName,IFlag
{
    
    [JsonConstructor]
    public DeviceDiscoveryMessage(string name,string flag)
    {
        Name = name;
        Flag = flag;
    }

    public string Name { get; }
    
    public string Flag { get; }
}