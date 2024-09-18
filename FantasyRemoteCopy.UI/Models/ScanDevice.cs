using FantasyRemoteCopy.Core.Enums;

using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Models;

/// <summary>
/// 扫描到的设备
/// </summary>
public record ScanDevice(SystemType SystemType, Device Device, string Flag, string DeviceName);
