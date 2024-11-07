using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public abstract class IconBase : ButtonBase, IIconElement
{
    PathF? IIconElement.IconPath { get; set; }

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;

    public string IconData
    {
        get => (string)GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }
}