using FantasyRemoteCopy.UI.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class DbUserService : UI.Interfaces.Impls.DbBase, IUserService
{
    protected override Task CreateTableAsync() => connection.CreateTableAsync<UserInfo>();

    public async Task<ResultBase<bool>> SaveUserAsync(UserInfo user)
    {
        var res = await connection.InsertAsync(user);
        return res > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("�����û�ʧ��");
    }

    public Task<ResultBase<UserInfo>> GetCurrentUserAsync() => throw new NotImplementedException();

    public Task<ResultBase<bool>> ClearUserAsync() => throw new NotImplementedException();
}