namespace Fantasy.UIKit.Interfaces;

public interface IImageElement
{
    ImageSource Source { get; set; }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
        typeof(ImageSource),
        typeof(IImageElement), default,
        propertyChanged: (b, o, v) => ((IUIKitElement)b).OnPropertyChanged()
    );
}