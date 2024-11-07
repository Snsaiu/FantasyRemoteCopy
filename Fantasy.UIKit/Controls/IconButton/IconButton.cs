﻿using System.Windows.Input;

namespace Fantasy.UIKit;

public partial class IconButton : IconBase, ICommandElement, IDrawable
{
    public IconButton()
    {
        this.SetDynamicResource(StyleProperty, "DefaultIconButtonStyle");
        this.Clicked += OnClicked;
    }

    private void OnClicked(object? sender, TouchEventArgs e)
    {
        if (this.Command is null)
            return;

        if (this.Command.CanExecute(this.CommandParameter))
        {
            Command.Execute(this.CommandParameter);
        }
    }

    public static readonly BindableProperty CommandProperty = ICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty = ICommandElement.CommandParameterProperty;

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
}