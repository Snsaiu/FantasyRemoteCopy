namespace Fantasy.UIKit.Interfaces;

public interface IContentElement
{
    public View Content { get; set; }

    public static BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View),
        typeof(IContentElement), default, propertyChanged: (b, o, v) => { ((IUIKitElement)b).OnPropertyChanged(); });
}