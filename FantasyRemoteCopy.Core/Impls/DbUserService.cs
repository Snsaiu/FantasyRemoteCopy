
using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls;

public class DbUserService : DbBase, IUserService
{


    public DbUserService()
    {

    }


    protected override Task CreateTableAsync()
    {
        return connection.CreateTableAsync<UserInfo>();
    }

    public async Task<ResultBase<bool>> SaveUserAsync(UserInfo user)
    {
        int res = await connection.InsertAsync(user);
        return res > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("≤Â»Î”√ªß ß∞‹");
    }

    public Task<ResultBase<UserInfo>> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResultBase<bool>> ClearUserAsync()
    {
        throw new NotImplementedException();
    }
}