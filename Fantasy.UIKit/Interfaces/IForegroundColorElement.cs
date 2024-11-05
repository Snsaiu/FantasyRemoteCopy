namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     可绘制前景色
/// </summary>
public interface IForegroundElement : IUIKitElement
{
    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create(nameof(ForegroundColor),
        typeof(Color), typeof(IForegroundElement), Colors.Black,
        propertyChanged: (bindable, obj, v) => ((IUIKitElement)bindable).OnPropertyChanged());

    public Color ForegroundColor { get; set; }
}