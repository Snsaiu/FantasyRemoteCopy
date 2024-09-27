using FantasyRemoteCopy.UI.Interfaces;

using Device = FantasyRemoteCopy.UI.Enums.Device;

namespace FantasyRemoteCopy.UI;
public sealed class DeviceTypeProvider : IDeviceType
{
    public Device Device { get; } = Device.Desktop;
}