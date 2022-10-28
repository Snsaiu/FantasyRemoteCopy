using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;

using System;

namespace FantasyRemoteCopy.Core.Impls;

public class DbSaveDataService:DbBase,ISaveDataService
{
    protected override async Task CreateTable()
    {
      await  this.connection.CreateTableAsync<UserInfo>();
    }

    public async Task<ResultBase<List<SaveDataModel>>> GetAllAsync()
    {
        try
        {
          var list= await  this.connection.Table<SaveDataModel>().ToListAsync();
          return new SuccessResultModel<List<SaveDataModel>>(list);
        }
        catch (Exception e)
        {

            return new ErrorResultModel<List<SaveDataModel>>(e.Message);
        }


    }

    public async Task<ResultBase<bool>> DeleteDataAsync(string guid)
    {
        
       int i= await this.connection.Table<SaveDataModel>().DeleteAsync(x=>x.Guid==guid);

       if (i > 0)
       {
           return new SuccessResultModel<bool>(true);
       }
       else
       {
           return new ErrorResultModel<bool>("清除失败！");
       }
    }

    public async Task<ResultBase<bool>> ClearAsync()
    {
        int i = await this.connection.Table<SaveDataModel>().DeleteAsync(x=>x.Guid!="");

        if (i > 0)
        {
            return new SuccessResultModel<bool>(true);
        }
        else
        {
            return new ErrorResultModel<bool>("清除失败！");
        }
    }

    public async Task<ResultBase<SaveDataModel>> AddAsync(SaveDataModel model)
    {
      int x=  await this.connection.InsertAsync(model);
      if (x > 0)
      {
          return new SuccessResultModel<SaveDataModel>(model);
      }

      return new ErrorResultModel<SaveDataModel>("插入失败");
    }
}