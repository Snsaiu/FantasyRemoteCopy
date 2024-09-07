using FantasyRemoteCopy.Core.Consts;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;

using System.Runtime.CompilerServices;
using System.Text;

namespace FantasyRemoteCopy.Core.Bussiness;



public class ReceiveBussiness
{
    private readonly IReceiveData _receiveData;
    private readonly ISendData _sendData;
    private readonly IUserService userService;

    
    public delegate void DiscoverEnableIpDelegate(SendInviteModel sendInviteModel);

    public delegate void SendErrorDelegate(string error);

    public delegate void ReceiveDataDelegate(TransformData data);

    public delegate void ReceivingDataDelegate(string ip);

    public delegate void ReceivedFileFinishedDelegate(string ip);

    public delegate void ReceivingProcessDelegate(string ip, double process);
    
    public ReceiveBussiness(IReceiveData receiveData, ISendData sendData, IUserService userService )
    {
        _receiveData = receiveData;
        _sendData = sendData;
        this.userService = userService;
        _receiveData.ReceiveInviteEvent += InviteHandle;
        _receiveData.ReceiveBuildConnectionEvent += BuildConnectionHandle;
        _receiveData.ReceiveDataEvent += d => { ReceiveDataEvent?.Invoke(d); };
        _receiveData.ReceivingDataEvent += ip => { ReceivingDataEvent?.Invoke(ip); };
        _receiveData.ReceivedFileFinishedEvent += ip => { ReceivedFileFinishedEvent?.Invoke(ip); };
        
        _receiveData.ReceivingProcessEvent += (ip, process) => { ReceivingProcessEvent?.Invoke(ip, process); };
        _receiveData.LiseningInvite();
        _receiveData.LiseningBuildConnection();
    }

    public event DiscoverEnableIpDelegate DiscoverEnableIpEvent;
    public event SendErrorDelegate SendErrorEvent;

    public event ReceiveDataDelegate ReceiveDataEvent;
    public event ReceivingDataDelegate ReceivingDataEvent;

    public event ReceivedFileFinishedDelegate ReceivedFileFinishedEvent;

    public event ReceivingProcessDelegate ReceivingProcessEvent;

    private async void BuildConnectionHandle(TransformData data)
    {
        if (data.Type == TransformType.RequestBuildConnect)
        {
            DataMetaModel? dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(data.Data));
            if (dmm is null)
                throw new NullReferenceException();

            dmm.State = MetaState.Received;
            ConstParams.ReceiveMetas.Add(dmm);

            await Task.Run(() => { _receiveData.LiseningData(data.TargetIp, dmm.Size); });

            data.Type = TransformType.BuildConnected;
            data.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dmm));

            await _sendData.SendBuildConnectionAsync(data);
        }
        else if (data.Type == TransformType.BuildConnected)
        {
            DataMetaModel? dmm = JsonConvert.DeserializeObject<DataMetaModel>(Encoding.UTF8.GetString(data.Data));
            if (dmm is null)
                throw new NullReferenceException();
            // �ҵ�Ԫ����
            DataMetaModel? findDmm = ConstParams.WillSendMetasQueue.FirstOrDefault(x => x.Guid == dmm.Guid);
            if (findDmm != null)
                if (dmm.State == MetaState.Received)
                {
                    findDmm.State = MetaState.Sending;
                    DataContent? detail = ConstParams.DataContents.FirstOrDefault(x => x.Guid == findDmm.Guid);
                    if (detail == null)
                    {
                        SendErrorEvent?.Invoke("sending data error ! because data is none!");
                        // ConstParams.DataContents.Remove(detail);
                        ConstParams.WillSendMetasQueue.Remove(findDmm);

                        return;
                    }

                    FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
                    if (userRes.Ok == false) throw new Exception(userRes.ErrorMsg);


                    await _sendData.SendDataAsync(findDmm, detail.Content, userRes.Data.DeviceNickName);
                }
        }
    }

    private void InviteHandle(TransformData data)
    {
        FantasyResultModel.ResultBase<UserInfo> userData = userService.GetCurrentUserAsync().GetAwaiter().GetResult();
        if (userData.Ok == false) return;

        SendInviteModel? sourceData = JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));
        if (sourceData is null)
            return;

        if (sourceData.MasterName != userData.Data.Name) return;


        if (data.Type == TransformType.ValidateAccount)
        {
            data.Type = TransformType.ReceiveValidateAccountResult;

            SendInviteModel sm = new SendInviteModel
            {
                MasterName = userData.Data.Name,
                DevicePlatform = DeviceInfo.Current.Platform.ToString(),
                DeviceName = DeviceInfo.Current.Name,
                NickName = userData.Data.DeviceNickName
            };

            string smStr = JsonConvert.SerializeObject(sm);

            data.Data = Encoding.UTF8.GetBytes(smStr);

            _sendData.SendInviteAsync(data);
        }
        else if (data.Type == TransformType.ReceiveValidateAccountResult)
        {
            SendInviteModel? sm = JsonConvert.DeserializeObject<SendInviteModel>(Encoding.UTF8.GetString(data.Data));
            if (sm is null)
                return;
            sm.DeviceIP = data.TargetIp;
            DiscoverEnableIpEvent?.Invoke(sm);
        }
    }
    
}