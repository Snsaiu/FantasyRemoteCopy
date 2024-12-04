using AirTransfer.Interfaces;

using Device = AirTransfer.Enums.Device;

namespace AirTransfer;

public sealed class DeviceTypeProvider : IDeviceType
{
    public Device Device { get; } = Device.Mobile;
}