using AirTransfer.Interfaces.Impls;

namespace AirTransfer
{
    public sealed class DefaultScanLocalNetIp(DeviceLocalIpBase deviceLocalIpBase)
        : LocalIpScannerBase(deviceLocalIpBase);
}