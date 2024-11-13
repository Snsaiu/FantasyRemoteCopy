using Fantasy.UIKit.Controls.Bases;

namespace Fantasy.UIKit;

public partial class ProgressBar : VisualBase, IValue<double>
{
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public BindableProperty ValueProperty = IValue<double>.ValueProperty;
}