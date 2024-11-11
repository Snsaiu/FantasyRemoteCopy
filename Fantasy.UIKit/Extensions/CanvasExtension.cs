using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics.Platform;

using IImage = Microsoft.Maui.Graphics.IImage;

#if !WINDOWS
using Microsoft.Maui.Graphics.Platform;

#else
using Microsoft.Maui.Graphics.Win2D;
#endif
namespace Fantasy.UIKit.Extensions;

internal static class CanvasExtension
{
    /// <summary>
    ///     绘制icon
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="element"></param>
    /// <param name="rect"></param>
    /// <param name="defaultSize"></param>
    /// <param name="scale"></param>
    internal static void DrawIcon(this ICanvas canvas, IIconElement element, RectF rect, int defaultSize, float scale)
    {
        if (element.IconPath == null)
            return;
        // canvas.StrokeColor = element.ForegroundColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.38f : 1f);
        canvas.FillColor = element.ForegroundColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.38f : 1f);
        using PathF path = element.IconPath.AsScaledPath(defaultSize / 24f * scale);
        float sx = rect.Center.X - defaultSize / 2 * scale;
        float sy = rect.Center.Y - defaultSize / 2 * scale;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    internal static CornerRadiusShape GetCornerRadiusShape(this ICornerRadiusShapeElement element, float width,
        float height)
    {
        return element.CornerRadiusShape.TopLeft is -1
               && element.CornerRadiusShape.TopRight is -1
               && element.CornerRadiusShape.BottomLeft is -1
               && element.CornerRadiusShape.BottomRight is -1
            ? Math.Min(width, height) / 2
            : element.CornerRadiusShape;
    }

    internal static void DrawBackground(this ICanvas canvas, IBackgroundElement element, RectF rect)
    {
        if (element.BackgroundColor == Colors.Transparent)
            return;
        canvas.FillColor =
            element.BackgroundColor.MultiplyAlpha(element.ViewState is ElementState.Disabled ? 0.12f : 1f);
        canvas.FillRectangle(rect);
    }

    internal static void DrawBorderColor(this ICanvas canvas, IBorderElement element, RectF rect)
    {
        if (element.BorderColor == Colors.Transparent || element.BorderThickness == 0)
            return;
        canvas.StrokeColor = element.BorderColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.12f : 1f);
        canvas.StrokeSize = element.BorderThickness;
        PathF path = element.GetClipPath(rect);
        canvas.DrawPath(path);
    }

    /// <summary>
    /// Gets IImage from image source stream for all platforms.
    /// </summary>
    /// <param name="source">Image source stream.</param>
    /// <returns>MAUI IImage implementation from the image stream.</returns>
    internal static IImage GetImageFromStream(Stream source)
    {
#if WINDOWS
            return new W2DImageLoadingService().FromStream(source);
#else
        return PlatformImage.FromStream(source);
#endif
    }

    internal static async Task<Stream> GetStreamFromImageSourceAsync(ImageSource imageSource)
    {
        switch (imageSource)
        {
            case FileImageSource fileImageSource:
                return await FileSystem.OpenAppPackageFileAsync(fileImageSource.File);

            case UriImageSource uriImageSource:
                using (HttpClient httpClient = new HttpClient())
                {
                    return await httpClient.GetStreamAsync(uriImageSource.Uri);
                }

            case StreamImageSource streamImageSource:
                return await streamImageSource.Stream(CancellationToken.None);

            default:
                throw new NotSupportedException("Unsupported ImageSource type");
        }
    }


    internal static void DrawPicture(this ICanvas canvas, IImageElement element, RectF rect)
    {
        GetStreamFromImageSourceAsync(element.Source).ContinueWith(x =>
        {
            Stream stream = x.GetAwaiter().GetResult();
            IImage image = GetImageFromStream(stream);
            canvas.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        });
    }

    internal static void DrawStateLayer(this ICanvas canvas, IStateLayerElement element, RectF rect,
        ElementState viewState)
    {
        if (viewState is ElementState.Hovered)
        {
            canvas.FillColor = element.StateColor.WithAlpha(StateLayerOpacity.Hovered);
            canvas.FillRectangle(rect);
        }
        else if (viewState is ElementState.Pressed)
        {
            canvas.FillColor = element.StateColor.WithAlpha(StateLayerOpacity.Pressed);
            canvas.FillRectangle(rect);
        }
    }

    internal static void DrawText<T>(this ICanvas canvas, T element, RectF rect,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment verticalAlignment = VerticalAlignment.Center) where T : ITextElement, IFontElement
    {
        if (rect is { Width: > 0, Height: > 0 })
        {
            canvas.DrawText(element, element.Text, rect, horizontalAlignment, verticalAlignment);
        }
    }

    internal static void DrawText<T>(this ICanvas canvas, T element, string text, RectF rect,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment verticalAlignment = VerticalAlignment.Center) where T : IFontElement
    {
        if (rect.Width < 0 || rect.Height < 0)
            return;
        FontStyleType style = element.FontIsItalic ? FontStyleType.Italic : FontStyleType.Normal;
        canvas.Font = new Microsoft.Maui.Graphics.Font(element.FontFamily, (int)element.FontWeight, style);
        canvas.FontColor = element.ForegroundColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.38f : 1f);
        canvas.FontSize = element.FontSize;
        canvas.DrawString(text, rect, horizontalAlignment, verticalAlignment);
    }


    internal static void DrawRipple(this ICanvas canvas, IRippleElement element, PointF point, float size,
        float percent)
    {
        canvas.FillColor = element.StateColor.WithAlpha(StateLayerOpacity.Pressed);
        canvas.FillCircle(point, 0f.Lerp(size, percent));
    }

    /// <summary>
    ///     获得剪裁后的path
    /// </summary>
    /// <param name="element"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    internal static PathF GetClipPath(this ICornerRadiusShapeElement element, RectF rect)
    {
        CornerRadiusShape radius = element.GetCornerRadiusShape(rect.Width, rect.Height);
        PathF path = new PathF();
        path.AppendRoundedRectangle(new RectF(rect.X, rect.Y, rect.Width, rect.Height),
            radius[0],
            radius[1],
            radius[2],
            radius[3],
            true
        );
        return path;
    }
}