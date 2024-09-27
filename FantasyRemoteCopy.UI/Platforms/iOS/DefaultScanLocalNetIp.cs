using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI
{
    public sealed class DefaultScanLocalNetIp(DeviceLocalIpBase deviceLocalIpBase)
        : LocalIpScannerBase(deviceLocalIpBase);
}