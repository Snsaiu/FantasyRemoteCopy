namespace Fantasy.UIKit.Interfaces;

public interface IBorderElement : ICornerRadiusShapeElement
{
    public Color BorderColor { get; set; }

    public readonly static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(IBorderElement), default, propertyChanged: (bindable, o, v) =>
    {
        ((IUIKitElement)bindable).OnPropertyChanged();
    });

    public int BorderThickness { get; set; }

    public readonly static BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(IBorderElement), default, propertyChanged: (bindable, o, v) =>
    {
        ((IUIKitElement)bindable).OnPropertyChanged();
    });

}