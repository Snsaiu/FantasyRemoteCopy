using FantasyRemoteCopy.UI.Interfaces;

using FantasyResultModel;

namespace FantasyRemoteCopy.UI
{
    public class DefaultScanLocalNetIp : IScanLocalNetIp
    {
        public Task<ResultBase<List<string>>> ScanLocalNetIpAsync()
        {
            throw new NotImplementedException();
        }
    }
}
