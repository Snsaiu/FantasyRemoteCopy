using FantasyResultModel;

namespace FantasyRemoteCopy.UI.Interfaces
{
    /// <summary>
    /// 获得本地Ip
    /// </summary>
    public interface IGetLocalIp
    {

        ResultBase<List<string>> GetLocalIp();

    }
}

