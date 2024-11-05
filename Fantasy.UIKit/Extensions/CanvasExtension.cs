#region

#endregion

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
        canvas.FillColor = element.BackgroundColor.MultiplyAlpha(element.ViewState is ElementState.Disabled ? 0.12f : 1f);
        canvas.FillRectangle(rect);
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