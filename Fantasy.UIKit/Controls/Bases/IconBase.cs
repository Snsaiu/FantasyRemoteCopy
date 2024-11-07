using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public abstract class IconBase : ButtonBase, IIconElement
{
    public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    PathF? IIconElement.IconPath { get; set; }

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;

    public string IconData
    {
        get => (string)GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public int BorderThickness
    {
        get => (int)GetValue(VisualBase.BorderThicknessProperty);
        set => SetValue(VisualBase.BorderThicknessProperty, value);
    }
}