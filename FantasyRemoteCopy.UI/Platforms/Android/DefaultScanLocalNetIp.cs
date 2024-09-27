using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;

using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace FantasyRemoteCopy.UI
{
    public sealed class DefaultScanLocalNetIp(DeviceLocalIpBase deviceLocalIpBase)
        : LocalIpScannerBase(deviceLocalIpBase)
    {
        public override async IAsyncEnumerable<ScanDevice> GetDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var localIp = await DeviceLocalIpBase.GetLocalIpAsync();
            var localSplits = localIp.Split(".")[0..3];
            var baseIP = string.Join(".", localSplits);
            for (var i = 1; i < 255; i++)
            {
                var ip = $"{baseIP}.{i}";
                var ping = new Ping();
                var reply = ping.Send(ip, 100);
                if (reply.Status == IPStatus.Success)
                {
                    yield return new ScanDevice(ip);
                }
            }
        }
    }
}