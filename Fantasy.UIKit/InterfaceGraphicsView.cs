#region

#endregion

namespace Fantasy.UIKit;

public class InterfaceGraphicsView : GraphicsView, IUIKitElement, IDisposable
{
    public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

    protected bool IsDispose;

    readonly IDispatcherTimer touchTimer;

    protected bool isTouching;

    protected PointF LastTouchPosition;


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
}