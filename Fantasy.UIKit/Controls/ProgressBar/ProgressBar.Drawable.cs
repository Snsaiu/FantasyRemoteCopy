namespace Fantasy.UIKit;

public partial class ProgressBar
{
    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();

        var progressValue = Math.Clamp(Value, 0f, 1f);

        // 算出进度条已经完成部分的宽度
        var calculateWidth = (float)(dirtyRect.Width * progressValue);
        var calculateRectF = new RectF(dirtyRect.X, dirtyRect.Y, calculateWidth, dirtyRect.Height);

        canvas.ClipPath(this.GetClipPath(dirtyRect));
        canvas.DrawBackground(this, dirtyRect);
        canvas.ResetState();
        canvas.ClipPath(this.GetClipPath(calculateRectF));
        canvas.DrawForeground(this, calculateRectF);
        canvas.ResetState();
    }
}