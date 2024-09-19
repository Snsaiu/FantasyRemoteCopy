using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyResultModel;

namespace FantasyRemoteCopy.UI
{
    public class DefaultScanLocalNetIp : IGetLocalNetDevices
    {
        public IAsyncEnumerable<ScanDevice> GetDevicesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
