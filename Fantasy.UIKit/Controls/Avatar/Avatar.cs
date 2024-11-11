using Fantasy.UIKit.Controls.Bases;

using System.ComponentModel;

namespace Fantasy.UIKit;

public partial class Avatar : VisualBase, IImageElement
{
    public Avatar()
    {
        SetDynamicResource(StyleProperty, "DefaultAvatarStyle");
    }

    [TypeConverter(typeof(ImageSourceConverter))]
    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty SourceProperty = IImageElement.SourceProperty;
}