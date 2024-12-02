using System.Windows.Input;

namespace Fantasy.UIKit.Controls.Bases;

public abstract class ButtonBase : VisualBase, ICommandElement, IForegroundElement
{
    protected ButtonBase()
    {
        Clicked += OnClicked;
    }

    private void OnClicked(object? sender, TouchEventArgs e)
    {
        if (Command is null)
            return;

        if (Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
        }
    }

    public static readonly BindableProperty CommandProperty = ICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty = ICommandElement.CommandParameterProperty;
    public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }


    public object CommandParameter
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void ChangeVisualState()
    {
        IsVisualStateChanging = true;
        var state = ViewState switch
        {
            ElementState.Normal => "normal",
            ElementState.Hovered => "hovered",
            ElementState.Pressed => "pressed",
            ElementState.Disabled => "disabled",
            _ => "normal"
        };

        VisualStateManager.GoToState(this, state);
        IsVisualStateChanging = false;
        Invalidate();
        InvalidateMeasure();
    }
}