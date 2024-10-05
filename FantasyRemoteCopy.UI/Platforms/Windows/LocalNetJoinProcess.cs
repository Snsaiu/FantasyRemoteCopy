using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI;

public sealed class LocalNetJoinProcess : LocalNetJoinProcessBase
{
    public LocalNetJoinProcess(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}