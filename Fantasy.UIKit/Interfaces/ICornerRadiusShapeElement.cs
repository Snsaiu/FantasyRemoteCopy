#region

using Fantasy.UIKit.Primitives;

#endregion

namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     支持圆角
/// </summary>
public interface ICornerRadiusShapeElement : IUIKitElement
{
    public static readonly BindableProperty CornerRadiusShapeProperty = BindableProperty.Create(nameof(Primitives.CornerRadiusShape),
        typeof(CornerRadiusShape), typeof(ICornerRadiusShapeElement), CornerRadiusShape.None,
        propertyChanged: (bindable, value, newValue) => { ((IUIKitElement)bindable).OnPropertyChanged(); });

    public CornerRadiusShape CornerRadiusShape { get; set; }
}