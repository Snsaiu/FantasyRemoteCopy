namespace Fantasy.UIKit.Interfaces;

public interface IValue<T>
{
    T Value { get; set; }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(T),
        typeof(IValue<T>), default, propertyChanged: (b, o, v) => { ((IUIKitElement)b).OnPropertyChanged(); });
}