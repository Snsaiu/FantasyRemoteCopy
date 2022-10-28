using FantasyRemoteCopy.UI.ViewModels;

namespace FantasyRemoteCopy.UI.Views;

public partial class ListPage : ContentPage
{
	public ListPage(ListPageModel vm)
	{
		InitializeComponent();

		this.BindingContext = vm;

	}
}