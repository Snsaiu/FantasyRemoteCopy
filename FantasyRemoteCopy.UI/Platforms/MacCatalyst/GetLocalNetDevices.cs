using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI;

public class GetLocalNetDevices:IGetLocalNetDevices
{
    public IAsyncEnumerable<ScanDevice> GetDevicesAsync(CancellationToken cancellationToken)
    {
        
    }
}