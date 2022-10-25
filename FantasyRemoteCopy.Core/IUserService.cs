
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface IUserService
{
    /// <summary>
    /// 保存用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task<ResultBase<bool>> SaveUser(UserInfo user);


    /// <summary>
    /// 获得当前用户
    /// </summary>
    /// <returns></returns>
    public Task<ResultBase<UserInfo>> GetCurrentUser();

    /// <summary>
    /// 清空当前用户
    /// </summary>
    /// <returns></returns>
    public Task<ResultBase<bool>> ClearUser();
}