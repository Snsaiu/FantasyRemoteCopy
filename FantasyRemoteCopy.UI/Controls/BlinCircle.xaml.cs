

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

    public static BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(BlinCircle));

    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

}