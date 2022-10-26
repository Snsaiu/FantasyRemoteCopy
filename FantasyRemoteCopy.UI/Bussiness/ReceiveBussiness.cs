using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

namespace FantasyRemoteCopy.UI.Bussiness;

public delegate void DiscoverEnableIpDelegate(string ip);

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
        //todo validate user 

        if (data.Type == DataType.ValidateAccount)
        {
            data.Type = DataType.ReceiveValidateAccountResult;
            
            this._sendData.SendInviteAsync(data);
        }
        else if (data.Type == DataType.ReceiveValidateAccountResult)
        {

            this.DiscoverEnableIpEvent?.Invoke(data.TargetIp);
        }
        
        
        

    }
}