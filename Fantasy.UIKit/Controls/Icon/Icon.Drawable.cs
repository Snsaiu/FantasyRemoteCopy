namespace Fantasy.UIKit;

public partial class Icon
{
    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.GetClipPath(dirtyRect));
        canvas.DrawBackground(this, dirtyRect);
        canvas.DrawBorderColor(this, dirtyRect);
        var scale = dirtyRect.Height / 40f;
        canvas.DrawIcon(this, dirtyRect, 24, scale);
        canvas.ResetState();
    }
}