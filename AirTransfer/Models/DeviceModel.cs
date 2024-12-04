#region

using AirTransfer.Enums;
using AirTransfer.Interfaces;

#endregion

namespace AirTransfer.Models;

public class DeviceModel : IFlag
{
    public string? DeviceName { get; set; }

    public string? DeviceType { get; set; }

    public string? NickName { get; set; }

    public SystemType SystemType { get; set; }

    public string? Flag { get; set; }
}