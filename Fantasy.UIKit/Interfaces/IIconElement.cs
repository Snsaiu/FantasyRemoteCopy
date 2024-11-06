namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     可设置图标
/// </summary>
public interface IIconElement : IBackgroundElement, IForegroundElement, IBorderElement
{
    public static readonly BindableProperty IconDataProperty = BindableProperty.Create(
        nameof(IconData),
        typeof(string),
        typeof(IIconElement),
        propertyChanged: (bindable, obj, v) =>
        {
            ((IIconElement)bindable).IconPath = PathBuilder.Build((string)v);
            ((IUIKitElement)bindable).InvalidateMeasure();
        });

    public PathF? IconPath { get; set; }

    public string IconData { get; set; }
}