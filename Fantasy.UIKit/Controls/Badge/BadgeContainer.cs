using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public partial class BadgeContainer : VisualBase, ITextElement, IFontElement
{
    public static readonly BindableProperty TextProperty = ITextElement.TextProperty;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;

    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    public static readonly BindableProperty FontIsItalicProperty = IFontElement.FontIsItalicProperty;

    public bool FontIsItalic
    {
        get => (bool)GetValue(FontIsItalicProperty);
        set => SetValue(FontIsItalicProperty, value);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var textSize = this.GetStringSize();
        if (textSize.Height < MinimumHeightRequest)
            textSize.Height = (float)MinimumHeightRequest;
        if (textSize.Width < MinimumWidthRequest)
            textSize.Width = (float)MinimumWidthRequest;
        DesiredSize = textSize;
        return textSize;
    }
}