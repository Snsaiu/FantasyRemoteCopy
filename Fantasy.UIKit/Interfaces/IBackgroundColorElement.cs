namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     设置形状的背景色
/// </summary>
public interface IBackgroundElement : ICornerRadiusShapeElement
{
    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(IBackgroundElement),
            propertyChanged: (bindable, value, newValue) => ((IUIKitElement)bindable).OnPropertyChanged());

    public Color BackgroundColor { get; set; }
}