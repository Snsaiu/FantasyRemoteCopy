using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface ISendData
{
    Task<ResultBase<bool>> SendRquestBuildConnectionDataAsync(TransformData data);
   

    Task<ResultBase<bool>> SendInviteAsync(TransformData data);
    Task<ResultBase<bool>> SendBuildConnectionAsync(TransformData data);
    Task<ResultBase<bool>> SendDataAsync(DataMetaModel data, string content);
}