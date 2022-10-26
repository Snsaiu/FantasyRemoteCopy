using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;

namespace FantasyRemoteCopy.UI.Bussiness;

public class ReceiveBussiness
{
    private readonly IReceiveData _receiveData;
    private readonly ISendData _sendData;

    public ReceiveBussiness(IReceiveData receiveData,ISendData sendData)
    {
        _receiveData = receiveData;
        _sendData = sendData;
        this._receiveData.ReceiveInviteEvent += InviteHandle;
        this._receiveData.LiseningInvite();
        this._receiveData.LiseningData();
    }
    
    

    public void InviteHandle(TransformData data)
    {

        TransformData td = new TransformData();

    }
}