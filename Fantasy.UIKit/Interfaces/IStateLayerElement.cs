namespace Fantasy.UIKit.Interfaces;

/// <summary>
/// 在原始的控件上附加一层状态
/// </summary>
public interface IStateLayerElement : ICornerRadiusShapeElement
{
    public Color StateColor { get; set; }

    public static BindableProperty StateColorProperty = BindableProperty.Create(nameof(StateColor), typeof(Color),
        typeof(IStateLayerElement), default, propertyChanged: (b, o, v) => ((IUIKitElement)b).OnPropertyChanged());
}