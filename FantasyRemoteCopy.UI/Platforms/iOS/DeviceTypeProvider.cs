using FantasyRemoteCopy.UI.Interfaces;
using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI;

public class DeivceTypeProvider:IDeviceType
{
    public Device Device { get; } = Device.Mobile;
}