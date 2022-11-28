using FantasyRemoteCopy.UI.Animation;
using FantasyRemoteCopy.UI.ViewModels;



namespace FantasyRemoteCopy.UI.Views;


public partial class HomePage : ContentPage
{
	public HomePage( )
	{
		InitializeComponent();

	
        downloadEllipse.LoopAnimation(TransformType.Fadein, 0, 1, 200, Easing.Default);
        downloadEllipseFill.LoopAnimation(TransformType.Fadein, 1, 0, 200, Easing.Default);

    }

    private void SearchClickEvent(object sender, EventArgs e)
    {
        this.searchBtn.OnceAninmation(TransformType.Fadein, 0.5, 1, 100, Easing.Linear);
    }


}