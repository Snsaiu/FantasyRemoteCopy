using Fantasy.UIKit.Converters;
using Fantasy.UIKit.Interfaces;
using Fantasy.UIKit.Primitives;

using System.ComponentModel;

namespace Fantasy.UIKit.Controls.Icon;

public class Icon : InterfaceGraphicsView, IIconElement
{
    public Icon()
    {
        IconDrawable drawable = new IconDrawable(this);
        Drawable = drawable;
    }

    public static readonly BindableProperty CornerRadiusProperty = ICornerRadiusShapeElement.CornerRadiusProperty;

    [TypeConverter(typeof(CornerRadiusShapeConverter))]
    public CornerRadiusShape CornerRadius { get => (CornerRadiusShape)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

    public static new readonly BindableProperty BackgroundColorProperty = IBackgroundElement.BackgroundColorProperty;
    public new Color BackgroundColor { get => (Color)GetValue(BackgroundColorProperty); set => SetValue(BackgroundColorProperty, value); }

    public static readonly BindableProperty ForeregroundColorProperty = IForegroundColorElement.ForegroundColorProperty;
    public Color ForegroundColor { get => (Color)GetValue(ForeregroundColorProperty); set => SetValue(ForeregroundColorProperty, value); }

    PathF? IIconElement.IconPath { get; set; }

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public string IconData { get => (string)GetValue(IconDataProperty); set => SetValue(IconDataProperty, value); }
}