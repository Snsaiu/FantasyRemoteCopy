namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     可绘制前景色
/// </summary>
internal interface IForegroundElement : IUIKitElement
{
    public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(ForegroundColor),
        typeof(Color), typeof(IForegroundElement), Colors.Black,
        propertyChanged: (bindable, obj, v) => ((IUIKitElement)bindable).OnPropertyChanged());

    public Color ForegroundColor { get; set; }
}