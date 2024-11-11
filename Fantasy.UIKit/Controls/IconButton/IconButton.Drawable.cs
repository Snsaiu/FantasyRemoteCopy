namespace Fantasy.UIKit
{
    public partial class IconButton
    {
        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.Antialias = true;
            canvas.ClipPath(this.GetClipPath(dirtyRect));
            canvas.DrawBackground(this, dirtyRect);
            canvas.DrawBorderColor(this, dirtyRect);

            float scale = dirtyRect.Height / 40f;
            canvas.DrawIcon(this, dirtyRect, 24, scale);

            if (this.RipplePercent is 0f or 1f)
            {
                canvas.DrawStateLayer(this, dirtyRect, this.ViewState);
            }
            else
            {
                canvas.DrawRipple(this, LastTouchPosition, RippleSize, RipplePercent);
            }


            canvas.ResetState();
        }
    }
}