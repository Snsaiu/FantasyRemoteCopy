﻿#region

using Fantasy.UIKit.Primitives;

#endregion

namespace Fantasy.UIKit.Interfaces;

/// <summary>
///     支持圆角
/// </summary>
internal interface ICornerRadiusShapeElement : IUIKitElement
{
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadiusShape),
        typeof(CornerRadiusShape), typeof(ICornerRadiusShapeElement), CornerRadiusShape.None,
        propertyChanged: (bindable, value, newValue) => { ((IUIKitElement)bindable).OnPropertyChanged(); });

    CornerRadiusShape CornerRadius { get; set; }
}