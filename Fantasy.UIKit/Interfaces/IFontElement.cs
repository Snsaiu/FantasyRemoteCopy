using System.Runtime.CompilerServices;

namespace Fantasy.UIKit.Interfaces;

public interface IFontElement : IForegroundElement
{
    float FontSize { get; set; }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float),
        typeof(IFontElement), 10.0f, propertyChanged: (b, o, v) => { ((IUIKitElement)b).InvalidateMeasure(); });

    string FontFamily { get; set; }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily),
        typeof(string),
        typeof(IFontElement), default,
        propertyChanged: (b, o, v) => { ((IUIKitElement)b).InvalidateMeasure(); });

    FontWeight FontWeight { get; set; }

    public static readonly BindableProperty FontWeightProperty = BindableProperty.Create(nameof(FontWeight),
        typeof(FontWeight), typeof(IFontElement), FontWeight.Regular,
        propertyChanged: (b, o, v) => { ((IUIKitElement)b).InvalidateMeasure(); });

    bool FontIsItalic { get; set; }

    public static readonly BindableProperty FontIsItalicProperty = BindableProperty.Create(nameof(FontIsItalic),
        typeof(bool), typeof(IFontElement),
        false, propertyChanged: (b, o, v) => { ((IUIKitElement)b).InvalidateMeasure(); });
}