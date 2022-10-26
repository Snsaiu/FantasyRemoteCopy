using System.Text.Json.Serialization;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Impls;

public class ConfigUserService:IUserService
{
    public async Task<ResultBase<bool>> SaveUser(UserInfo user)
    {
        Preferences.Default.Set<string>("user", JsonConvert.SerializeObject(user));
       return new SuccessResultModel<bool>(true);
    }

    public async Task<ResultBase<UserInfo>> GetCurrentUser()
    {
       string v=  Preferences.Default.Get<string>("user","");
       if (string.IsNullOrEmpty(v))
       {
           return new ErrorResultModel<UserInfo>("未发现用户信息");
       }

       var user= JsonConvert.DeserializeObject<UserInfo>(v);
       return new SuccessResultModel<UserInfo>(user);
    }

    public async Task<ResultBase<bool>> ClearUser()
    {
         Preferences.Default.Clear();
        return new SuccessResultModel<bool>(true);
    }
}