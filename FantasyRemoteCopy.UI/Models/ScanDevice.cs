using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Models;

/// <summary>
/// 扫描到的设备
/// </summary>
public class ScanDevice(SystemType systemType, Device device, string flag, string? deviceName):IFlag
{
    public SystemType SystemType { get; init; } = systemType;
    public Device Device { get; init; } = device;
    public string Flag { get; init; } = flag;
    
    public string? DeviceName { get; init; } = deviceName;
    
}

public class JoinMessageModel(SystemType systemType, Device device, string flag, string? deviceName,string sendTarget) : ScanDevice(systemType, device, flag, deviceName)
{
    public string SendTarget { get; init; } = sendTarget;
}

