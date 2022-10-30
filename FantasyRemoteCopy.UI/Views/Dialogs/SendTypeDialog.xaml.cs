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
            this.CanBeDismissedByTappingOutsideOfPopup = false;
        
          var f=await FilePicker.PickAsync();
           
          if (f != null)
          {

              FileInfo finfo = new FileInfo(f.FullPath);
              if (finfo.Length > 1073741824)
              {
                    
                  await App.Current.MainPage.DisplayAlert("Warning", "File cannot be larger than 1G ！", "Ok");
                  return;
              }
              this.act.IsVisible = true;
              var res= await this._sendData.SendData(this.discoveredDeviceModel.Ip, f.FullPath, DataType.File);

              if (res != null)
              {
                  this.act.IsVisible = false;
#if WINDOWS
                  await Application.Current.MainPage.DisplayAlert("Information", "Sended!", "Ok");
#endif


                    this.Close();

                }



            }
            this.Close();

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
     
    }
}