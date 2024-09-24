using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;

using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace FantasyRemoteCopy.UI
{
    public class DefaultScanLocalNetIp : LocalIpScannerBase
    {
        public DefaultScanLocalNetIp(DeviceLocalIpBase deviceLocalIpBase) : base(deviceLocalIpBase)
        {
        }

        public override async IAsyncEnumerable<ScanDevice> GetDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            string localIp = await DeviceLocalIpBase.GetLocalIpAsync();
            string[] localSplits = localIp.Split(".")[0..3];
            string baseIP = string.Join(".", localSplits);
            for (int i = 1; i < 255; i++)
            {
                string ip = $"{baseIP}.{i}";
                Ping ping = new Ping();
                PingReply reply = ping.Send(ip, 100);

                if (reply.Status == IPStatus.Success)
                {
                    yield return new ScanDevice(ip);
                }
            }

        }
    }
}
