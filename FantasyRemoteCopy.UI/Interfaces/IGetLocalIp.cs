using FantasyResultModel;

namespace FantasyRemoteCopy.UI.Interfaces
{


    /// <summary>
    /// 获得本机ip
    /// </summary>
    public interface IGetLocalIp
    {
        Task<string> GetLocalIpAsync();
    }
    
    
    //
    // /// 获得本地Ip
    // /// </summary>
    // public interface IGetLocalIp
    // {
    //
    //     ResultBase<List<string>> GetLocalIp();
    //
    // }
}

