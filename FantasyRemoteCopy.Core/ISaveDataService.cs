using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface ISaveDataService
{
    Task<ResultBase<List<SaveDataModel>>> GetAllAsync();

   Task< ResultBase<bool>> DeleteDataAsync(string guid);

   Task< ResultBase<bool>> ClearAsync();

   Task< ResultBase<SaveDataModel>> AddAsync(SaveDataModel model);


}