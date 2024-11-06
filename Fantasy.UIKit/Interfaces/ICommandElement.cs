using System.Windows.Input;

namespace Fantasy.UIKit.Interfaces;

public interface ICommandElement
{
    public ICommand Command { get; set; }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ICommandElement), default);

    public object CommandParameter { get; set; }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ICommandElement), null);
}