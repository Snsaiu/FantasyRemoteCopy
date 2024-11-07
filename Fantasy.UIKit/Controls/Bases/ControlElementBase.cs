using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy.UIKit.Controls.Bases
{
    /// <summary>
    /// 所有控件的基类
    /// </summary>
    public abstract class ControlElementBase : InterfaceGraphicsView, IDrawable
    {
        protected ControlElementBase()
        {
            this.Drawable = this;
        }

        public abstract void Draw(ICanvas canvas, RectF dirtyRect);
    }
}