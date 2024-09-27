using FantasyResultModel;
using FantasyResultModel.Impls;

using SaveDataModel = FantasyRemoteCopy.UI.Models.SaveDataModel;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class DbSaveDataService : DbBase, ISaveDataService
{
    protected override  Task CreateTableAsync() => connection.CreateTableAsync<SaveDataModel>();

    public async Task<ResultBase<List<SaveDataModel>>> GetAllAsync()
    {
        try
        {
            var list = await connection.Table<SaveDataModel>().ToListAsync();
            return new SuccessResultModel<List<SaveDataModel>>(list);
        }
        catch (Exception e)
        {
            return new ErrorResultModel<List<SaveDataModel>>(e.Message);
        }
    }

    public async Task<ResultBase<bool>> DeleteDataAsync(string guid)
    {
        var i = await connection.Table<SaveDataModel>().DeleteAsync(x => x.Guid == guid);
        return i > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("清除失败！");
    }

    public async Task<ResultBase<bool>> ClearAsync()
    {
        var i = await connection.Table<SaveDataModel>().DeleteAsync(x => x.Guid != "");
        return i > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("清除失败！");
    }

    public async Task<ResultBase<SaveDataModel>> AddAsync(SaveDataModel model)
    {
        var x = await connection.InsertAsync(model);
        return x > 0 ? new SuccessResultModel<SaveDataModel>(model) : new ErrorResultModel<SaveDataModel>("插入失败");
    }
}