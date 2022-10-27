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
        this.toLabel.Text ="To£º" + this.discoveredDeviceModel.NickName;
    }

}