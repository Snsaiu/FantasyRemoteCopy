using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public partial class ProgressBar : VisualBase, IValue<double>, IForegroundElement
{
    public ProgressBar()
    {
        SetDynamicResource(StyleProperty, "DefaultProgressBarStyle");
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }


    public static readonly BindableProperty ValueProperty = IValue<double>.ValueProperty;

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;

    void IUIKitElement.OnPropertyChanged()
    {
        Invalidate();
    }
}