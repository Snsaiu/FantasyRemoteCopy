using CommunityToolkit.Maui.Views;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Views.Dialogs;

public partial class SendTypeDialog : Popup
{
    private readonly SendDataBussiness _sendData;

    public SendTypeDialog(SendDataBussiness sendData)
    {
        _sendData = sendData;
        InitializeComponent();
    }

    private DiscoveredDeviceModel discoveredDeviceModel;

    public void InitData(DiscoveredDeviceModel model)
    {
        this.discoveredDeviceModel = model;
        this.toLabel.Text ="To：" + this.discoveredDeviceModel.NickName;
    }

    private async void TextInputEvent(object sender, EventArgs e)
    {
        
        var inputPage= App.Current.Services.GetService<TextInputPage>();
        inputPage.InitData(discoveredDeviceModel);
        await Application.Current.MainPage.Navigation.PushAsync(inputPage);
        this.Close();
    }

    private  async void FileInputEvent(object sender, EventArgs e)
    {
        try
        {
        
          var f=await FilePicker.PickAsync();
          if (f != null)
          {
             
              await this._sendData.SendData(this.discoveredDeviceModel.Ip, f.FullPath, DataType.File);
           

            }
           

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
     
    }
}