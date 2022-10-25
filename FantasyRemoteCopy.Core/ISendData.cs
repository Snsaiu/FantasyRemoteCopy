using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface ISendData
{
    Task<ResultBase<bool>> SendDataAsync(TransformData data);

    Task<ResultBase<bool>> SendInviteAsync(TransformData data);
}