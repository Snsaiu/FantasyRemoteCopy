
using System.ComponentModel;

namespace Fantasy.UIKit.Controls.Icon;

public class Icon : InterfaceGraphicsView, IIconElement
{
    public Icon()
    {
        SetDynamicResource(StyleProperty, "DefaultIconStyle");
        IconDrawable drawable = new IconDrawable(this);
        Drawable = drawable;
    }

    public static readonly BindableProperty CornerRadiusShapeProperty = ICornerRadiusShapeElement.CornerRadiusShapeProperty;

    [TypeConverter(typeof(CornerRadiusShapeConverter))]
    public CornerRadiusShape CornerRadiusShape { get => (CornerRadiusShape)GetValue(CornerRadiusShapeProperty); set => SetValue(CornerRadiusShapeProperty, value); }


    public static new readonly BindableProperty BackgroundColorProperty = IBackgroundElement.BackgroundColorProperty;
    public new Color BackgroundColor { get => (Color)GetValue(BackgroundColorProperty); set => SetValue(BackgroundColorProperty, value); }

    public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;
    public Color ForegroundColor { get => (Color)GetValue(ForegroundColorProperty); set => SetValue(ForegroundColorProperty, value); }

    PathF? IIconElement.IconPath { get; set; }

    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public string IconData { get => (string)GetValue(IconDataProperty); set => SetValue(IconDataProperty, value); }
}