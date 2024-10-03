using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using Device = FantasyRemoteCopy.UI.Enums.Device;

namespace FantasyRemoteCopy.UI.Models;

public class JoinMessageModel(SystemType systemType, Device device, string flag, string? deviceName, string sendTarget,string name) : ScanDevice(systemType, device, flag, deviceName),IName
{
    public string SendTarget { get; init; } = sendTarget;
    public string Name { get; init; } = name;
}