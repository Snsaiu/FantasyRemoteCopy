
using FantasyResultModel;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;


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

    protected override bool ExistTable()
    {
        var exist= this.connection.Table<UserInfo>();
        if (exist == null)
            return false;
        return true;
    }

    public Task<ResultBase<bool>> SaveUser(UserInfo user)
    {
        throw new NotImplementedException();
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