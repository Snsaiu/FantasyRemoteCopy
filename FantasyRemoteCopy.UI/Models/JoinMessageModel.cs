using FantasyRemoteCopy.UI.Enums;
using Device = FantasyRemoteCopy.UI.Enums.Device;

namespace FantasyRemoteCopy.UI.Models;

public class JoinMessageModel(SystemType systemType, Device device, string flag, string? deviceName, string sendTarget) : ScanDevice(systemType, device, flag, deviceName)
{
    public string SendTarget { get; init; } = sendTarget;
}