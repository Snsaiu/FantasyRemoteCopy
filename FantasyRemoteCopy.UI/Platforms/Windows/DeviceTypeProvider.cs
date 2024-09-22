using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public class DeviceTypeProvider:IDeviceType
{
    public Device Device { get; }= Device.Desktop;
}