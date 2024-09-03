using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

using FantasyResultModel;
using FantasyResultModel.Impls;

using Newtonsoft.Json;

using System.Text;

namespace FantasyRemoteCopy.Core.Bussiness;

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

public class SendDataBussiness
{
    private readonly IScanLocalNetIp _scanLocalNetIp;
    private readonly ISendData _sendData;
    private readonly IUserService _userService;

    public SendDataBussiness(IScanLocalNetIp scanLocalNetIp,
        ISendData sendData,
        IUserService userService)
    {
        _scanLocalNetIp = scanLocalNetIp;
        _sendData = sendData;
        _userService = userService;

        _sendData.SendingDataEvent += (ip) =>
        {
            SendingDataEvent?.Invoke(ip);
        };
        _sendData.SendFinishedEvent += (ip) =>
        {
            SendFinishedEvent?.Invoke(ip);
        };
    }

    public event SendingDataDelegate SendingDataEvent;

    public event SendFinishedDelegate SendFinishedEvent;

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="targetip">目标ip，要发送的设备的ip地址</param>
    /// <param name="content">发送内容，如果datatype 是file或者image 那么content是文件路径，否则是文本内容</param>
    /// <param name="datatype">content的类型</param>
    /// <returns></returns>
    public async Task<ResultBase<bool>> SendData(string targetip, string content, DataType datatype)
    {
        TransformData tf = new TransformData
        {
            DataGuid = Guid.NewGuid().ToString()
        };

        await Task.Delay(1000);

        long contentSize = 0;

        if (datatype == DataType.Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            contentSize = bytes.Length;
        }
        else
        {
            FileInfo f = new FileInfo(content);
            contentSize = f.Length;

        }



        tf.TargetIp = targetip;

        ResultBase<UserInfo> userRes = await _userService.GetCurrentUserAsync();
        if (userRes.Ok == false)
            return new ErrorResultModel<bool>(userRes.ErrorMsg ?? string.Empty);

        tf.TargetDeviceNickName = userRes.Data.DeviceNickName;

        DataMetaModel dm = new DataMetaModel
        {
            Guid = tf.DataGuid,
            Size = contentSize,
            State = MetaState.Receiving,
            DataType = datatype,
            TargetIp = tf.TargetIp
        };
        dm.Guid = tf.DataGuid;
        if (datatype == DataType.File)
        {
            dm.SourcePosition = content;
        }



        tf.Type = TransformType.RequestBuildConnect;
        tf.Port = ConstParams.BuildTcpIp_Port;

        tf.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dm));

        ConstParams.WillSendMetasQueue.Add(dm);

        ConstParams.DataContents.Add(new DataContent { Guid = dm.Guid, Content = content });
        await _sendData.SendRquestBuildConnectionDataAsync(tf);
        return new SuccessResultModel<bool>(true);
    }

    /// <summary>
    /// 设备发现
    /// </summary>
    /// <returns></returns>
    public async Task<ResultBase<bool>> DeviceDiscover()
    {
        ResultBase<List<string>> scanRes = await _scanLocalNetIp.ScanLocalNetIpAsync();
        if (scanRes.Ok == false)
            return new ErrorResultModel<bool>(scanRes.ErrorMsg ?? string.Empty);

        ResultBase<UserInfo> userRes = await _userService.GetCurrentUserAsync();
        if (userRes.Ok == false)
            return new ErrorResultModel<bool>(userRes.ErrorMsg ?? string.Empty);



        foreach (string ip in scanRes.Data)
        {
            try
            {
                SendInviteModel sm = new SendInviteModel
                {
                    MasterName = userRes.Data.Name,
                    DevicePlatform = DeviceInfo.Current.Platform.ToString(),
                    DeviceName = DeviceInfo.Current.Name,
                    NickName = userRes.Data.DeviceNickName
                };
                string smStr = JsonConvert.SerializeObject(sm);

                TransformData td = new TransformData
                {
                    Data = Encoding.UTF8.GetBytes(smStr),
                    Type = TransformType.ValidateAccount,
                    TargetIp = ip,
                    Port = ConstParams.INVITE_PORT
                };
                await _sendData.SendInviteAsync(td);

            }
            catch (Exception)
            {

            }

        }

        return await Task.FromResult(new SuccessResultModel<bool>(true));

    }
}