using FantasyRemoteCopy.UI.Models;

using FantasyResultModel;


namespace FantasyRemoteCopy.UI.Interfaces;

public interface ISaveDataService
{
    Task<ResultBase<List<SaveDataModel>>> GetAllAsync();

    Task<ResultBase<bool>> DeleteDataAsync(string guid);

    Task<ResultBase<bool>> ClearAsync();

    Task<ResultBase<SaveDataModel>> AddAsync(SaveDataModel model);


}