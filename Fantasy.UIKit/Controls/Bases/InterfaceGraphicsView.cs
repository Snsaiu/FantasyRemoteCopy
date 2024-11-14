using Microsoft.Maui.Animations;

using System.ComponentModel;

namespace Fantasy.UIKit;

public class InterfaceGraphicsView : GraphicsView, IRippleElement, IDisposable
{
    public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

    protected bool IsDispose;
    private readonly IDispatcherTimer touchTimer;

    protected bool isTouching;

    internal PointF LastTouchPosition { get; set; }

    internal float RipplePercent { get; set; }

    protected IAnimationManager animationManager;
    private ElementState _viewState = ElementState.Normal;

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

        touchTimer = Dispatcher.CreateTimer();
        touchTimer.Interval = TimeSpan.FromMilliseconds(500);
        touchTimer.IsRepeating = false;
        touchTimer.Tick += (s, e) =>
        {
            if (LongPressed != null)
            {
            }
        };
    }

    protected virtual float GetRippleSize()
    {
        var points = new PointF[4];
        points[0].X = points[2].X = LastTouchPosition.X;
        points[0].Y = points[1].Y = LastTouchPosition.Y;
        points[1].X = points[3].X = LastTouchPosition.X - (float)Bounds.Width;
        points[2].Y = points[3].Y = LastTouchPosition.Y - (float)Bounds.Height;
        var maxSize = 0f;
        foreach (var point in points)
        {
            var size = MathF.Pow(
                MathF.Pow(point.X - LastTouchPosition.X, 2f)
                + MathF.Pow(point.Y - LastTouchPosition.Y, 2f),
                0.5f
            );
            if (size > maxSize)
            {
                maxSize = size;
            }
        }

        return maxSize;
    }


    protected void StartRippleAnimation()
    {
        animationManager ??= Handler?.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        animationManager?.Add(new Microsoft.Maui.Animations.Animation(callback: (progress) =>
        {
            RipplePercent = 0f.Lerp(1f, progress);
            Invalidate();
        }, duration: RippleDuration, easing: RippleEasing));
    }

    protected bool IsVisualStateChanging { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    public ElementState ViewState
    {
        get => _viewState;
        set
        {
            _viewState = value;
            ChangeVisualState();
        }
    }

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
        if (!Enable)
            return;
        if (!e.IsInsideBounds)
            ViewState = ElementState.Normal;
    }

    protected virtual void InterfaceGraphicsView_EndHoverInteraction(object? sender, EventArgs e)
    {
        if (!Enable)
            return;
        ViewState = ElementState.Normal;
    }

    protected virtual void InterfaceGraphicsView_StartHoverInteraction(object? sender, TouchEventArgs e)
    {
        if (!Enable)
            return;
        LastTouchPosition = e.Touches[0];
        ViewState = ElementState.Hovered;
    }

    protected virtual void InterfaceGraphicsView_CancelInteraction(object? sender, EventArgs e)
    {
        if (!Enable)
            return;
        ViewState = ElementState.Normal;
        isTouching = false;
        touchTimer.Stop();
    }

    protected virtual void InterfaceGraphicsView_EndInteraction(object? sender, TouchEventArgs e)
    {
        Released?.Invoke(sender, e);
        Clicked?.Invoke(sender, e);
#if __MOBILE__
        ViewState = ElementState.Normal;
#else
        this.ViewState = e.IsInsideBounds ? ElementState.Hovered : ElementState.Normal;
#endif


        isTouching = false;
        touchTimer.Stop();
    }

    protected virtual void InterfaceGraphicsView_StartInteraction(object? sender, TouchEventArgs e)
    {
        ViewState = ElementState.Pressed;
        LastTouchPosition = e.Touches[0];

        RippleSize = GetRippleSize();
        StartRippleAnimation();
        Pressed?.Invoke(this, e);
        isTouching = true;
        touchTimer.Start();
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