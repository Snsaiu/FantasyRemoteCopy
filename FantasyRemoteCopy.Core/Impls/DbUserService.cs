
using FantasyResultModel;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls;

public class DbUserService:DbBase,IUserService
{
    

    public DbUserService()
    {
        
    }
    

    protected override void CreateTable()
    {
        this.connection.CreateTable<UserInfo>();
    }



    public async Task<ResultBase<bool>> SaveUser(UserInfo user)
    {
        int res = 0;
        await Task.Run(() =>
        {
           res=  this.connection.Insert(user);
        });
        if (res > 0)
        {
            return new SuccessResultModel<bool>(true);
        }
        return new ErrorResultModel<bool>("≤Â»Î”√ªß ß∞‹");

    }

    public Task<ResultBase<UserInfo>> GetCurrentUser()
    {
        throw new NotImplementedException();
    }

    public Task<ResultBase<bool>> ClearUser()
    {
        throw new NotImplementedException();
    }
}