using System.ComponentModel;

namespace Fantasy.UIKit;

public partial class Icon : IconBase
{
    public Icon()
    {
        SetDynamicResource(StyleProperty, "DefaultIconStyle");
    }
}