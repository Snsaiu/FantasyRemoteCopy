using Fantasy.UIKit.Extensions;

namespace Fantasy.UIKit.Controls.Icon;

public class IconDrawable(Icon view) : IDrawable
{
    private readonly Icon _view = view;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(_view.GetClipPath(dirtyRect));
        float scale = dirtyRect.Height / 40f;
        canvas.DrawIcon(_view, dirtyRect, 24, scale);
        canvas.ResetState();

    }
}