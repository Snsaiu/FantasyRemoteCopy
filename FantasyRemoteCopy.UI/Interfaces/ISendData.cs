using FantasyResultModel;

using DataMetaModel = FantasyRemoteCopy.UI.Models.DataMetaModel;
using TransformData = FantasyRemoteCopy.UI.Models.TransformData;

namespace FantasyRemoteCopy.UI.Interfaces;


/// <summary>
/// 发送文件的委托
/// </summary>
/// <param name="ip"></param>
public delegate void SendingDataDelegate(string ip);

/// <summary>
/// 发送完成的委托
/// </summary>
/// <param name="ip"></param>
public delegate void SendFinishedDelegate(string ip);



public interface ISendData
{

    event SendingDataDelegate SendingDataEvent;

    event SendFinishedDelegate SendFinishedEvent;

    Task<ResultBase<bool>> SendRquestBuildConnectionDataAsync(TransformData data);


    Task<ResultBase<bool>> SendInviteAsync(TransformData data);
    Task<ResultBase<bool>> SendBuildConnectionAsync(TransformData data);
    Task<ResultBase<bool>> SendDataAsync(DataMetaModel data, string content, string deviceNickName);
}