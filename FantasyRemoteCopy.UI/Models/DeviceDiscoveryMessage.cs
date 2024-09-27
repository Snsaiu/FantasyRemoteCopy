using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class DeviceDiscoveryMessage(string name, string flag,string targetFlag) : IName, IFlag,ITargetFlag
{
    public string Name { get; } = name;

    public string Flag { get; } = flag;
    public string TargetFlag { get; } = targetFlag;
}