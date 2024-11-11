namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     针对有标题的控件，可以自定义标题内容
/// </summary>
public interface IHeaderElement : IUIKitElement
{
    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(View),
        typeof(IHeaderElement),
        propertyChanged: (bindable, value, newValue) => ((IUIKitElement)bindable).InvalidateMeasure());

    public View Header { get; set; }
}