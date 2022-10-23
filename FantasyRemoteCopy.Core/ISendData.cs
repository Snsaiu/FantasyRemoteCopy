using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;

namespace FantasyRemoteCopy.Core;

public interface ISendData
{
    Task<ResultBase<bool>> SendAsync(TransformData data);
}