using System.Text;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;

using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Bussiness;

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


    public async Task<ResultBase<bool>> SendData(TransformData data)
    {
        data.Type = DataType.BuildConnected;
        data.Port = ConstParams.TcpIp_Port;
       await  this._sendData.SendDataAsync(data);
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

                string smStr=JsonConvert.SerializeObject(sm);

                TransformData td = new TransformData();


                td.Data = Encoding.UTF8.GetBytes(smStr);
                td.Type = DataType.ValidateAccount;
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