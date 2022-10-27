using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;
using System.Text;
using FantasyRemoteCopy.Core.Consts;

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
        this._receiveData.ReceiveBuildConnectionEvent += BuildConnectionHandle;
        this._receiveData.LiseningInvite();
        this._receiveData.LiseningBuildConnection();
    }

    private void BuildConnectionHandle(TransformData data)
    {
        if (data.Type == DataType.BuildConnected)
        {
            Task.Run(() =>
            {
                DataMetaModel dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(data.Data));
                ConstParams.ReceiveMetas.Add(dmm);

                this._receiveData.LiseningData(data.TargetIp, dmm.Size);
            });
            data.Type= DataType.BuildConnected;
            this._sendData.SendBuildConnectionAsync(data);

        }
        else if(data.Type == DataType.BuildConnected) 
        {

            // ·¢ËÍÊý¾Ý

        }


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
            sm.DevicePlatform = DeviceInfo.Current.Platform.ToString();
            sm.DeviceName = DeviceInfo.Current.Name;
         
            string smStr = JsonConvert.SerializeObject(sm);

            data.Data = Encoding.UTF8.GetBytes(smStr);
            
            this._sendData.SendInviteAsync(data);
        }
        else if (data.Type == DataType.ReceiveValidateAccountResult)
        {

            var sm= JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));
            sm.DeviceIP = data.TargetIp;
            this.DiscoverEnableIpEvent?.Invoke(sm);
        }
        
        
        

    }
}