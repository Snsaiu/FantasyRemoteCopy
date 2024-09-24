using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class DeviceDiscoveryMessage(string name, string flag) : IName, IFlag
{
    public string Name { get; } = name;

    public string Flag { get; } = flag;
}