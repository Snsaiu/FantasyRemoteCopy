using FantasyResultModel;

namespace FantasyRemoteCopy.UI.Interfaces;

public interface IGlobalScanLocalNetIp
{
    Task<ResultBase<bool>> GlobalSearch();
}