using FantasyRemoteCopy.UI.ViewModels;

using Fantasy.Maui.Controls;
using Fantasy.Maui.Controls.Animation;

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
        var g = sender as Grid;
        g.OnceAninmation(TransformType.Fadein, 0.5, 1, 100, Easing.Linear);
    }
}