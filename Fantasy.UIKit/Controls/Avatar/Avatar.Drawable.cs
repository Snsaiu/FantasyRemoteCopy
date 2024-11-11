namespace Fantasy.UIKit;

public partial class Avatar
{
    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.GetClipPath(dirtyRect));
        if (Source is null)
        {
            canvas.DrawBackground(this, dirtyRect);
        }
        else
        {
            canvas.DrawPicture(this, dirtyRect);
        }

        canvas.DrawBorderColor(this, dirtyRect);

        // 如果没有图片，那么显示背景色


        canvas.ResetState();
    }
}