using Microsoft.Maui.Graphics.Platform;

namespace Fantasy.UIKit.Extensions;

internal static class IFontElementExtension
{
    internal static SizeF GetStringSize<TElement>(this TElement element) where TElement : ITextElement, IFontElement
    {
        return element.GetStringSize(element.Text);
    }

    internal static SizeF GetStringSize<TElement>(this TElement element, string text) where TElement : IFontElement
    {
        if (string.IsNullOrEmpty(text))
            return Size.Zero;
        var weight = (int)element.FontWeight;
        var style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;

        var font = new Microsoft.Maui.Graphics.Font(element.FontFamily, weight, style);


        var fontService = new PlatformStringSizeService();
        var size = fontService.GetStringSize(text, font, element.FontSize);

        return new SizeF(MathF.Ceiling(size.Width), MathF.Ceiling(size.Height));
    }
}