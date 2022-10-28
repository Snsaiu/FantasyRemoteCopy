using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;
using System.Text;
using FantasyRemoteCopy.Core.Consts;

namespace FantasyRemoteCopy.Core.Bussiness;

public delegate void DiscoverEnableIpDelegate(SendInviteModel sendInviteModel);

public delegate void SendErrorDelegate(string error);

public delegate void ReceiveDataDelegate(TransformData data);

public class ReceiveBussiness
{
    private readonly IReceiveData _receiveData;
    private readonly ISendData _sendData;
    private readonly IUserService userService;

    public event DiscoverEnableIpDelegate DiscoverEnableIpEvent;
   public event SendErrorDelegate SendErrorEvent;

    public event ReceiveDataDelegate ReceiveDataEvent;

    public ReceiveBussiness(IReceiveData receiveData,ISendData sendData,IUserService userService)
    {
        _receiveData = receiveData;
        _sendData = sendData;
        this.userService = userService;
        this._receiveData.ReceiveInviteEvent += InviteHandle;
        this._receiveData.ReceiveBuildConnectionEvent += BuildConnectionHandle;
        this._receiveData.ReceiveDataEvent += (d) =>
        {
            this.ReceiveDataEvent?.Invoke(d);
        };

        this._receiveData.LiseningInvite();
        this._receiveData.LiseningBuildConnection();


    }

    private async void BuildConnectionHandle(TransformData data)
    {
        if (data.Type == TransformType.RequestBuildConnect)
        {
            DataMetaModel dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(data.Data));
            dmm.State = MetaState.Received;
            ConstParams.ReceiveMetas.Add(dmm);

            await Task.Run(() =>
            {
                 this._receiveData.LiseningData(data.TargetIp, dmm.Size);
        });

        data.Type= TransformType.BuildConnected;
            data.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dmm));

           await this._sendData.SendBuildConnectionAsync(data);
        }
        else if(data.Type == TransformType.BuildConnected) 
        {

            DataMetaModel dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(data.Data));
            // 找到元数据
            var findDmm= ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == dmm.Guid);
            if(findDmm!=null)
            {
                if(dmm.State==MetaState.Received)
                {
                    findDmm.State = MetaState.Sending;
                    var detail = ConstParams.DataContents.FirstOrDefault(x => x.Guid == findDmm.Guid);
                    if (detail == null)
                    {
                        this.SendErrorEvent?.Invoke("sending data error ! because data is none!");
                        ConstParams.DataContents.Remove(detail);
                        ConstParams.WillSendMetasQueue.Remove(findDmm);

                        return;
                    }

                    var userRes = await this.userService.GetCurrentUser();
                    if (userRes.Ok == false)
                    {
                        throw new Exception(userRes.ErrorMsg);
                    }


                 await this._sendData.SendDataAsync(findDmm,detail.Content,userRes.Data.DeviceNickName );
                }
            }

        }
    }




    public void InviteHandle(TransformData data)
    {

        var userData= this.userService.GetCurrentUser().GetAwaiter().GetResult();
        if (userData.Ok == false)
        {
            return;
        }

        var sourceData= JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));

        if (sourceData.MasterName != userData.Data.Name)
        {
            return;
        }


        if (data.Type == TransformType.ValidateAccount)
        {
            data.Type = TransformType.ReceiveValidateAccountResult;

            SendInviteModel sm = new SendInviteModel();
            sm.MasterName = userData.Data.Name;
            sm.DevicePlatform = DeviceInfo.Current.Platform.ToString();
            sm.DeviceName = DeviceInfo.Current.Name;
            sm.NickName = userData.Data.DeviceNickName;
         
            string smStr = JsonConvert.SerializeObject(sm);

            data.Data = Encoding.UTF8.GetBytes(smStr);
            
            this._sendData.SendInviteAsync(data);
        }
        else if (data.Type == TransformType.ReceiveValidateAccountResult)
        {

            var sm= JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));
            sm.DeviceIP = data.TargetIp;
            this.DiscoverEnableIpEvent?.Invoke(sm);
        }
        
        
        

    }
}