using Fantasy.UIKit.Controls.Bases;
using System.Windows.Input;

namespace Fantasy.UIKit;

public partial class Button : ButtonBase, ITextElement, IForegroundElement

{
    public Button()
    {
        this.SetDynamicResource(StyleProperty, "");
    }

    public string Text { get; set; }
    public Color ForegroundColor { get; set; }
}