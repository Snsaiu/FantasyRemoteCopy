using Fantasy.UIKit.Controls.Bases;

using System.ComponentModel;
using System.Windows.Input;

namespace Fantasy.UIKit;

[ContentProperty(nameof(Content))]
public class Card : ContainerBase, IBackgroundElement, IBorderElement, ICommandElement
{
    public Card()
    {
    }

    protected override void OnPropertyChanged()
    {
        throw new NotImplementedException();
    }

    public static readonly BindableProperty CommandProperty = ICommandElement.CommandProperty;

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = ICommandElement.CommandParameterProperty;

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}