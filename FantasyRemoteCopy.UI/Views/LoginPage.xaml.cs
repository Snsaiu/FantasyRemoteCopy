using FantasyRemoteCopy.UI.ViewModels;

namespace FantasyRemoteCopy.UI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageModel pagemodel)
	{
		InitializeComponent();
		this.BindingContext = pagemodel;
	}
}