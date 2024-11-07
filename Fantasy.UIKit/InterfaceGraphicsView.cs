#region

#endregion

using System.ComponentModel;

namespace Fantasy.UIKit;

public class InterfaceGraphicsView : GraphicsView, IUIKitElement, IStateLayerElement, IRippleElement, IDisposable
{
    public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

    protected bool IsDispose;

    readonly IDispatcherTimer touchTimer;

    protected bool isTouching;

    protected PointF LastTouchPosition;

    internal float RippleSize { get; set; }

    public event EventHandler<TouchEventArgs> Clicked;
    public event EventHandler<TouchEventArgs> Pressed;
    public event EventHandler<TouchEventArgs> Released;
    public event EventHandler<TouchEventArgs> LongPressed;

#if WINDOWS || MACCATALYST
    public event EventHandler<TouchEventArgs> RightClicked;
#endif


    public InterfaceGraphicsView()
    {
        StartInteraction += InterfaceGraphicsView_StartInteraction;
        EndInteraction += InterfaceGraphicsView_EndInteraction;
        CancelInteraction += InterfaceGraphicsView_CancelInteraction;
        StartHoverInteraction += InterfaceGraphicsView_StartHoverInteraction;
        EndHoverInteraction += InterfaceGraphicsView_EndHoverInteraction;
        MoveHoverInteraction += InterfaceGraphicsView_MoveHoverInteraction;

        this.touchTimer = this.Dispatcher.CreateTimer();
        this.touchTimer.Interval = TimeSpan.FromMilliseconds(500);
        this.touchTimer.IsRepeating = false;
        this.touchTimer.Tick += (s, e) =>
        {
            if (this.LongPressed != null)
            {
            }
        };
    }

    protected virtual float GetRippleSize()
    {
        var points = new PointF[4];
        points[0].X = points[2].X = this.LastTouchPosition.X;
        points[0].Y = points[1].Y = this.LastTouchPosition.Y;
        points[1].X = points[3].X = this.LastTouchPosition.X - (float)this.Bounds.Width;
        points[2].Y = points[3].Y = this.LastTouchPosition.Y - (float)this.Bounds.Height;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - this.LastTouchPosition.X, 2f)
                + MathF.Pow(point.Y - this.LastTouchPosition.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }

        return maxSize;
    }


    protected bool IsVisualStateChanging { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ElementState ViewState { get; set; }

    public bool Enable
    {
        get => (bool)GetValue(EnableProperty);
        set => SetValue(EnableProperty, value);
    }

    void IUIKitElement.OnPropertyChanged()
    {
        if (Handler is not null && IsVisualStateChanging)
            Invalidate();
    }

    protected virtual void InterfaceGraphicsView_MoveHoverInteraction(object? sender, TouchEventArgs e)
    {
    }

    protected virtual void InterfaceGraphicsView_EndHoverInteraction(object? sender, EventArgs e)
    {
    }

    protected virtual void InterfaceGraphicsView_StartHoverInteraction(object? sender, TouchEventArgs e)
    {
    }

    protected virtual void InterfaceGraphicsView_CancelInteraction(object? sender, EventArgs e)
    {
    }

    protected virtual void InterfaceGraphicsView_EndInteraction(object? sender, TouchEventArgs e)
    {
        this.Released?.Invoke(sender, e);
        this.Clicked?.Invoke(sender, e);

        this.isTouching = false;
        this.touchTimer.Stop();
    }

    protected virtual void InterfaceGraphicsView_StartInteraction(object? sender, TouchEventArgs e)
    {
        this.ViewState = ElementState.Pressed;
        LastTouchPosition = e.Touches[0];

        this.RippleSize = this.GetRippleSize();


        this.Pressed?.Invoke(this, e);
        this.isTouching = true;
        this.touchTimer.Start();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDispose)
        {
            if (disposing)
            {
                StartInteraction -= InterfaceGraphicsView_StartInteraction;
                EndInteraction -= InterfaceGraphicsView_EndInteraction;
                CancelInteraction -= InterfaceGraphicsView_CancelInteraction;
                StartHoverInteraction -= InterfaceGraphicsView_StartHoverInteraction;
                EndHoverInteraction -= InterfaceGraphicsView_EndHoverInteraction;
                MoveHoverInteraction -= InterfaceGraphicsView_MoveHoverInteraction;
            }

            IsDispose = true;
        }
    }

    public static readonly BindableProperty StateColorProperty = IStateLayerElement.StateColorProperty;

    public Color StateColor
    {
        get => (Color)GetValue(StateColorProperty);
        set => SetValue(StateColorProperty, value);
    }

    public static readonly BindableProperty RippleDurationProperty = IRippleElement.RippleDurationProperty;

    public double RippleDuration
    {
        get => (double)GetValue(RippleDurationProperty);
        set => SetValue(RippleDurationProperty, value);
    }

    public static readonly BindableProperty RippleEasingProperty = IRippleElement.RippleEasingProperty;

    public Easing RippleEasing
    {
        get => (Easing)GetValue(RippleEasingProperty);
        set => SetValue(RippleEasingProperty, value);
    }


    public static readonly BindableProperty CornerRadiusShapeProperty =
        ICornerRadiusShapeElement.CornerRadiusShapeProperty;

    [TypeConverter(typeof(CornerRadiusShapeConverter))]
    public CornerRadiusShape CornerRadiusShape
    {
        get => (CornerRadiusShape)GetValue(CornerRadiusShapeProperty);
        set => SetValue(CornerRadiusShapeProperty, value);
    }
}