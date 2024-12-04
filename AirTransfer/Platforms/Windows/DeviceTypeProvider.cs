using AirTransfer.Interfaces;

using Device = FantasyRemoteCopy.UI.Enums.Device;

namespace AirTransfer;
public sealed class DeviceTypeProvider : IDeviceType
{
    public Device Device { get; } = Device.Desktop;
}