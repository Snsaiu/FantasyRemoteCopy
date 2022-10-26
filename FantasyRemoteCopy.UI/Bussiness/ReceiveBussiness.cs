using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;
using System.Text;

namespace FantasyRemoteCopy.UI.Bussiness;

public delegate void DiscoverEnableIpDelegate(SendInviteModel sendInviteModel);

public class ReceiveBussiness
{
    private readonly IReceiveData _receiveData;
    private readonly ISendData _sendData;
    private readonly IUserService userService;

    public event DiscoverEnableIpDelegate DiscoverEnableIpEvent;
   

    public ReceiveBussiness(IReceiveData receiveData,ISendData sendData,IUserService userService)
    {
        _receiveData = receiveData;
        _sendData = sendData;
        this.userService = userService;
        this._receiveData.ReceiveInviteEvent += InviteHandle;
        this._receiveData.LiseningInvite();
        this._receiveData.LiseningData();
    }
    
    

    public void InviteHandle(TransformData data)
    {

        var userData= this.userService.GetCurrentUser().GetAwaiter().GetResult();
        if (userData.Ok == false)
        {
            return;
        }

        if (data.Type == DataType.ValidateAccount)
        {
            data.Type = DataType.ReceiveValidateAccountResult;

            SendInviteModel sm = new SendInviteModel();
            sm.MasterName = userData.Data.Name;
            sm.DevicePlatform = DeviceInfo.Current.Platform;
            sm.DeviceName = DeviceInfo.Current.Name;

            string smStr = JsonConvert.SerializeObject(sm);

            data.Data = Encoding.UTF8.GetBytes(smStr);
            
            this._sendData.SendInviteAsync(data);
        }
        else if (data.Type == DataType.ReceiveValidateAccountResult)
        {

            var sm= JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));

            this.DiscoverEnableIpEvent?.Invoke(sm);
        }
        
        
        

    }
}