using System.ComponentModel;

namespace Fantasy.UIKit;

public class Icon : IconBase
{
    public Icon()
    {
        SetDynamicResource(StyleProperty, "DefaultIconStyle");
        IconDrawable drawable = new IconDrawable(this);
        Drawable = drawable;
    }
}