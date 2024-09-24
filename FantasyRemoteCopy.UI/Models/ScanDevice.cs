using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;

using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Models;

/// <summary>
/// 扫描到的设备
/// </summary>
public class ScanDevice : IFlag
{
    /// <summary>
    /// 扫描到的设备
    /// </summary>
    public ScanDevice(SystemType systemType, Device device, string flag, string? deviceName)
    {
        SystemType = systemType;
        Device = device;
        Flag = flag;
        DeviceName = deviceName;
    }
    public ScanDevice(string flag)
    {
        Flag = flag;
    }

    public SystemType SystemType { get; init; } = SystemType.None;
    public Device Device { get; init; } = Device.None;
    public string Flag { get; init; }

    public string? DeviceName { get; init; }

}