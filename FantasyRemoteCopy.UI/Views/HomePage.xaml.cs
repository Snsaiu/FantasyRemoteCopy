using FantasyRemoteCopy.UI.ViewModels;

namespace FantasyRemoteCopy.UI.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    private void SearchClickEvent(object sender, EventArgs e)
    {

    }
}