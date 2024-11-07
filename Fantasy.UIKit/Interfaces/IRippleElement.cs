namespace Fantasy.UIKit.Interfaces;

/// <summary>
/// 涟漪动画
/// </summary>
public interface IRippleElement : IStateLayerElement
{
    double RippleDuration { get; set; }

    Easing RippleEasing { get; set; }

    public static readonly BindableProperty RippleDurationProperty = BindableProperty.Create(nameof(RippleDuration),
        typeof(double), typeof(IRippleElement), 0.9);

    public static readonly BindableProperty RippleEasingProperty = BindableProperty.Create(nameof(RippleEasing),
        typeof(Easing), typeof(IRippleElement),
        Easing.SinInOut);
}