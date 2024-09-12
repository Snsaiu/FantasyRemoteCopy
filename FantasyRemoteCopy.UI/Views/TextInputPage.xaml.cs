using FantasyRemoteCopy.UI.Models;

using SendDataBussiness = FantasyRemoteCopy.UI.Bussiness.SendDataBussiness;

namespace FantasyRemoteCopy.UI.Views;

public partial class TextInputPage : ContentPage
{
    private readonly SendDataBussiness _sendData;

    private DiscoveredDeviceModel discoveredDeviceModel;



    public void InitData(DiscoveredDeviceModel model)
    {
        discoveredDeviceModel = model;
        cp.Title = "To：" + discoveredDeviceModel.NickName;
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
        sendBtn.IsEnabled = !string.IsNullOrWhiteSpace(edit.Text);
    }
}