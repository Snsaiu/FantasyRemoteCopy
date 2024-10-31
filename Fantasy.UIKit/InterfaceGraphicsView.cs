using Fantasy.UIKit.Interfaces;

namespace Fantasy.UIKit;

internal class InterfaceGraphicsView : GraphicsView, IUIKitElement, IDisposable
{
    public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

    public bool Enable
    {
        get => (bool)GetValue(EnableProperty);
        set => SetValue(EnableProperty, value);
    }

    public InterfaceGraphicsView()
    {
        StartInteraction += InterfaceGraphicsView_StartInteraction;
        EndInteraction += InterfaceGraphicsView_EndInteraction;
        CancelInteraction += InterfaceGraphicsView_CancelInteraction;
        StartHoverInteraction += InterfaceGraphicsView_StartHoverInteraction;
        EndHoverInteraction += InterfaceGraphicsView_EndHoverInteraction;
        MoveHoverInteraction += InterfaceGraphicsView_MoveHoverInteraction;

    }

    protected virtual void InterfaceGraphicsView_MoveHoverInteraction(object? sender, TouchEventArgs e)
    {
        throw new NotImplementedException();
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
    }

    protected virtual void InterfaceGraphicsView_StartInteraction(object? sender, TouchEventArgs e)
    {
    }

    protected bool IsDispose;
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}