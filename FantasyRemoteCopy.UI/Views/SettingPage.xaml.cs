using FantasyRemoteCopy.UI.ViewModels;

namespace FantasyRemoteCopy.UI.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingPageModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}