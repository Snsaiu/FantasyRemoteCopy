using FantasyResultModel;
namespace FantasyRemoteCopy.UI.Interfaces
{
    /// <summary>
    /// 获得本地局域网的所有ip
    /// </summary>
    public interface IScanLocalNetIp
    {
        Task<ResultBase<List<string>>> ScanLocalNetIpAsync();
    }
}

