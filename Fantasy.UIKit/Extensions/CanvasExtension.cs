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

    /// <summary>
    /// 获得剪裁后的path
    /// </summary>
    /// <param name="element"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    internal static PathF GetClipPath(this ICornerRadiusShapeElement element, RectF rect)
    {
        var radius = element.
    }
}