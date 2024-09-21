using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Views;

public partial class TextInputPage : ContentPage
{
   
    private DiscoveredDeviceModel discoveredDeviceModel;



    public void InitData(DiscoveredDeviceModel model)
    {
        discoveredDeviceModel = model;
        cp.Title = "To：" + discoveredDeviceModel.NickName;
    }


    public TextInputPage()
    {
       
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