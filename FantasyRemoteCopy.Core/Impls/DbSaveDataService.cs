using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls;

public class DbSaveDataService : DbBase, ISaveDataService
{
    protected override async Task CreateTableAsync()
    {
        await connection.CreateTableAsync<SaveDataModel>();
    }

    public async Task<ResultBase<List<SaveDataModel>>> GetAllAsync()
    {
        try
        {
            List<SaveDataModel> list = await connection.Table<SaveDataModel>().ToListAsync();
            return new SuccessResultModel<List<SaveDataModel>>(list);
        }
        catch (Exception e)
        {

            return new ErrorResultModel<List<SaveDataModel>>(e.Message);
        }


    }

    public async Task<ResultBase<bool>> DeleteDataAsync(string guid)
    {

        int i = await connection.Table<SaveDataModel>().DeleteAsync(x => x.Guid == guid);

        return i > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("清除失败！");
    }

    public async Task<ResultBase<bool>> ClearAsync()
    {
        int i = await connection.Table<SaveDataModel>().DeleteAsync(x => x.Guid != "");

        return i > 0 ? new SuccessResultModel<bool>(true) : new ErrorResultModel<bool>("清除失败！");
    }

    public async Task<ResultBase<SaveDataModel>> AddAsync(SaveDataModel model)
    {
        int x = await connection.InsertAsync(model);
        return x > 0 ? new SuccessResultModel<SaveDataModel>(model) : new ErrorResultModel<SaveDataModel>("插入失败");
    }
}