namespace Fantasy.UIKit;

[ContentProperty(nameof(Content))]
public class Card : TemplatedView, IContentElement
{
    public static readonly BindableProperty ContentProperty = IContentElement.ContentProperty;

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}