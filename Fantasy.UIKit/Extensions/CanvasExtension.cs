#region

using Fantasy.UIKit.Interfaces;
using Fantasy.UIKit.Primitives;

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
        canvas.StrokeColor = element.ForegroundColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.38f : 1f);
        canvas.StrokeColor = element.BackgroundColor.WithAlpha(element.ViewState is ElementState.Disabled ? 0.38f : 1f);
        using var path = element.IconPath.AsScaledPath(defaultSize / 24f * scale);
        var sx = rect.Center.X - defaultSize / 2 * scale;
        var sy = rect.Center.Y - defaultSize / 2 * scale;
        path.Move(sx, sy);
        canvas.FillPath(path);
    }

    internal static CornerRadiusShape GetCornerRadiusShape(this ICornerRadiusShapeElement element, float width,
        float height) =>
        element.CornerRadius.TopLeft is -1
        && element.CornerRadius.TopRight is -1
        && element.CornerRadius.BottomLeft is -1
        && element.CornerRadius.BottomRight is -1
            ? Math.Min(width, height) / 2
            : element.CornerRadius;


    /// <summary>
    ///     获得剪裁后的path
    /// </summary>
    /// <param name="element"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    internal static PathF GetClipPath(this ICornerRadiusShapeElement element, RectF rect)
    {
        var radius = element.GetCornerRadiusShape(rect.Width, rect.Height);
        var path = new PathF();
        path.AppendRoundedRectangle(new RectF(rect.Center.X, rect.Center.Y, rect.Width, rect.Height),
            radius[0],
            radius[1],
            radius[2],
            radius[3],
            true
        );
        return path;
    }
}