using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI
{
    public class DefaultScanLocalNetIp : LocalIpScannerBase
    {
        public DefaultScanLocalNetIp(DeviceLocalIpBase deviceLocalIpBase) : base(deviceLocalIpBase)
        {
        }
    }
}