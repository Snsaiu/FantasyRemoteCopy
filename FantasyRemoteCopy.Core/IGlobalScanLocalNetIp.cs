using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface IGlobalScanLocalNetIp
{
    Task<ResultBase<bool>> GlobalSearch();
}