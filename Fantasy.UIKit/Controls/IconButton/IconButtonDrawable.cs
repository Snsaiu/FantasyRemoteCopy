namespace Fantasy.UIKit.Controls.IconButton
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

        }
    }
}
