using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public partial class Button : ButtonBase, ITextElement, IFontElement
{
    public Button()
    {
        SetDynamicResource(StyleProperty, "DefaultButtonStyle");
    }

    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;

    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;

    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;

    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;

    public static readonly BindableProperty FontIsItalicProperty = IFontElement.FontIsItalicProperty;

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        // 获得文字size
        var textSize = this.GetStringSize();
        if (textSize.Height < MinimumHeightRequest)
            textSize.Height = (float)MinimumHeightRequest;
        if (textSize.Width < MinimumWidthRequest)
            textSize.Width = (float)MinimumWidthRequest;

        textSize.Width += 48f;
        // textSize.Height += 40f;

        DesiredSize = textSize;
        return textSize;
    }


    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    public bool FontIsItalic
    {
        get => (bool)GetValue(FontIsItalicProperty);
        set => SetValue(FontIsItalicProperty, value);
    }
}