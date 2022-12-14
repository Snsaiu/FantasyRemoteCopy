using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Views;

public partial class TextInputPage : ContentPage
{
    private readonly SendDataBussiness _sendData;

    private DiscoveredDeviceModel discoveredDeviceModel;



    public void InitData(DiscoveredDeviceModel model)
    {
        this.discoveredDeviceModel = model;
        this.cp.Title = "To：" + this.discoveredDeviceModel.NickName;
    }
    

    public TextInputPage(SendDataBussiness sendData)
    {
        _sendData = sendData;
        InitializeComponent();
    }


    /// <summary>
    /// �ı�ʧȥ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnFocusedEvent(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(this.edit.Text))
        {
            this.sendBtn.IsEnabled=false;
        }
        else
        {
            this.sendBtn.IsEnabled=true;
        }
    }
}