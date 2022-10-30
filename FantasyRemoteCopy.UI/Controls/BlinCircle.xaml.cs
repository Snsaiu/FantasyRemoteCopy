

using FantasyRemoteCopy.UI.Animation;

namespace FantasyRemoteCopy.UI.Controls;

public partial class BlinCircle : ContentView
{
	public BlinCircle()
	{
		InitializeComponent();
        ine.LoopAnimation(TransformType.Fadein, 0, 1, 200, Easing.Default);
        oute.LoopAnimation(TransformType.Fadein, 1, 0, 200, Easing.Default);
    }
}