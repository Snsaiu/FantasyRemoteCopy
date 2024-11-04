namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     可以显示文本
/// </summary>
public interface ITextElement : IUIKitElement
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
        typeof(ITextElement),
        propertyChanged: (bindable, value, newValue) => ((IUIKitElement)bindable).InvalidateMeasure());

    public string Text { get; set; }
}