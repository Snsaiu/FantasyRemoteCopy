namespace Fantasy.UIKit.Controls.Bases;

public abstract class VisualBase : ControlElementBase, IBackgroundElement, IBorderElement
{
    public static readonly BindableProperty BorderColorProperty = IBorderElement.BorderColorProperty;

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static new readonly BindableProperty BackgroundColorProperty = IBackgroundElement.BackgroundColorProperty;

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }


    public static readonly BindableProperty BorderThicknessProperty = IBorderElement.BorderThicknessProperty;

    public int BorderThickness
    {
        get => (int)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }
}