using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls;

public class ConfigUserService : IUserService
{
    public Task<ResultBase<bool>> SaveUserAsync(UserInfo user)
    {
        Preferences.Default.Set<string>("user", JsonConvert.SerializeObject(user));
        return Task.FromResult<ResultBase<bool>>(new SuccessResultModel<bool>(true));
    }

    public async Task<ResultBase<UserInfo>> GetCurrentUserAsync()
    {
        string v = Preferences.Default.Get<string>("user", "");
        if (string.IsNullOrEmpty(v))
        {
            return new ErrorResultModel<UserInfo>("未发现用户信息");
        }

        UserInfo user = JsonConvert.DeserializeObject<UserInfo>(v);
        return new SuccessResultModel<UserInfo>(user);
    }

    public async Task<ResultBase<bool>> ClearUserAsync()
    {
        Preferences.Default.Clear();
        return new SuccessResultModel<bool>(true);
    }
}