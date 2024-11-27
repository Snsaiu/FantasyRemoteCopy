namespace Fantasy.UIKit;

public partial class BadgeContainer
{
    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        //如果没有文字，那么就不要绘制
        if (string.IsNullOrEmpty(Text))
            return;
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.GetClipPath(dirtyRect));
        canvas.DrawBackground(this, dirtyRect);
        canvas.DrawText(this, dirtyRect);
        canvas.ResetState();
    }
}