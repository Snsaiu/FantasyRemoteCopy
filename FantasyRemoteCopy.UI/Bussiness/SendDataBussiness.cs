using System.Text;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;
using FantasyResultModel;
using FantasyResultModel.Impls;

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
                TransformData td = new TransformData();
                td.Data = Encoding.UTF8.GetBytes(userRes.Data.Name);
                td.Type = DataType.ValidateAccount;
                td.TargetIp = ip;
                td.Port = "5976";
                await this._sendData.SendInviteAsync(td);

            }
            catch (Exception ex)
            {

            }

       }

       return await Task.FromResult(new SuccessResultModel<bool>(true));

    }
}