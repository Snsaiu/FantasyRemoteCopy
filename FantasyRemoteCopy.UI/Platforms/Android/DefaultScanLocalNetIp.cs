using FantasyRemoteCopy.UI.Interfaces;

using FantasyResultModel;

namespace FantasyRemoteCopy.UI
{
    public class DefaultScanLocalNetIp : IScanLocalNetIp
    {
        private readonly IGetLocalIp _getLocalIp;

        public DefaultScanLocalNetIp(IGetLocalIp getLocalIp)
        {
            _getLocalIp = getLocalIp;
        }


        public Task<ResultBase<List<string>>>? ScanLocalNetIpAsync()
        {
            _getLocalIp.GetLocalIp();
            return null;
        }
    }
}
