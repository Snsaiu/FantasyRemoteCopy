using System.Text;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;

using Newtonsoft.Json;

namespace FantasyRemoteCopy.Core.Bussiness;

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
    }


    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="targetip">目标ip，要发送的设备的ip地址</param>
    /// <param name="content">发送内容，如果datatype 是file或者image 那么content是文件路径，否则是文本内容</param>
    /// <param name="datatype">content的类型</param>
    /// <returns></returns>
    public async Task<ResultBase<bool>> SendData(string targetip, string content, DataType datatype)
    {
        var tf = new TransformData ();
        tf.DataGuid = Guid.NewGuid().ToString();

        await Task.Delay(1000);

        long contentSize = 0;

        if (datatype == DataType.Text)
        {
          var bytes= Encoding.UTF8.GetBytes(content);
          contentSize= bytes.Length;
        }
        else
        {
          FileInfo f=new FileInfo(content);
          contentSize = f.Length;
            
        }
       


        tf.TargetIp = targetip;

        var userRes = await this._userService.GetCurrentUser();
        if (userRes.Ok == false)
            return new ErrorResultModel<bool>(userRes.ErrorMsg);

        tf.TargetDeviceNickName = userRes.Data.DeviceNickName;

        DataMetaModel dm = new DataMetaModel { Guid = tf.DataGuid, Size = contentSize, State = MetaState.Receiving };
        dm.DataType = datatype;
        dm.TargetIp = tf.TargetIp;
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
        await  this._sendData.SendRquestBuildConnectionDataAsync(tf);
        return null;
    }
    
    /// <summary>
    /// 设备发现
    /// </summary>
    /// <returns></returns>
    public async Task<ResultBase<bool>> DeviceDiscover()
    {
       var scanRes= await this._scanLocalNetIp.ScanLocalNetIpAsync();
       if (scanRes.Ok == false)
           return new ErrorResultModel<bool>(scanRes.ErrorMsg);
      
       var userRes= await this._userService.GetCurrentUser();
       if (userRes.Ok == false)
           return new ErrorResultModel<bool>(userRes.ErrorMsg);

       
  
       foreach (string ip in scanRes.Data)
       {

            try
            {
                
                SendInviteModel sm=new SendInviteModel();
                sm.MasterName = userRes.Data.Name;
                sm.DevicePlatform = DeviceInfo.Current.Platform.ToString();
                sm.DeviceName = DeviceInfo.Current.Name;
                sm.NickName = userRes.Data.DeviceNickName;
                string smStr=JsonConvert.SerializeObject(sm);

                TransformData td = new TransformData();


                td.Data = Encoding.UTF8.GetBytes(smStr);
                td.Type = TransformType.ValidateAccount;
                td.TargetIp = ip;
                td.Port = ConstParams.INVITE_PORT;
                await this._sendData.SendInviteAsync(td);

            }
            catch (Exception ex)
            {

            }

       }

       return await Task.FromResult(new SuccessResultModel<bool>(true));

    }
}