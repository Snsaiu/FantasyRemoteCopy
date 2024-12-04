using AirTransfer.Enums;
using AirTransfer.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;

using FantasyRemoteCopy.UI.Interfaces;

namespace AirTransfer.Models;

public class DeviceModel : IFlag
{
    public string? DeviceName { get; set; }

    public string? DeviceType { get; set; }

    public string? Flag { get; set; }

    public string? NickName { get; set; }

    public SystemType SystemType { get; set; }
}