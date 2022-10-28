using CommunityToolkit.Maui.Views;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Views.Dialogs;

public partial class SendTypeDialog : Popup
{
	public SendTypeDialog()
	{
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
}