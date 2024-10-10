using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
namespace FantasyRemoteCopy.UI;

public sealed class LocalNetJoinProcess : LocalNetJoinProcessBase
{
    public LocalNetJoinProcess(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}