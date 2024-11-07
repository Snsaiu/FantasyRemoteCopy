namespace Fantasy.UIKit
{
    internal class IconButtonDrawable : IDrawable
    {
        private readonly IconButton _view;

        public IconButtonDrawable(IconButton view)
        {
            _view = view;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.Antialias = true;
            canvas.ClipPath(_view.GetClipPath(dirtyRect));
            canvas.DrawBackground(_view, dirtyRect);
            canvas.DrawBorderColor(_view, dirtyRect);

            float scale = dirtyRect.Height / 40f;
            canvas.DrawIcon(_view, dirtyRect, 24, scale);

            if (_view.RipplePercent is 0f or 1f)
            {
                canvas.DrawStateLayer(_view, dirtyRect, _view.ViewState);
            }
            else
            {
                canvas.DrawRipple(_view, _view.LastTouchPosition, _view.RippleSize, _view.RipplePercent);
            }


            canvas.ResetState();
        }
    }
}