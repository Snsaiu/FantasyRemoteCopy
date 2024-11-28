using System.Windows.Input;
using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit
{
    public partial class CardContainer : VisualBase, ICommandElement
    {
        public static readonly BindableProperty CommandProperty = ICommandElement.CommandProperty;

        public static readonly BindableProperty CommandParameterProperty = ICommandElement.CommandParameterProperty;

        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
        public object CommandParameter { get => (object)GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
    }
}