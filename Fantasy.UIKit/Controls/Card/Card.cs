

using System.ComponentModel;
using System.Windows.Input;

namespace Fantasy.UIKit;

[ContentProperty(nameof(Content))]
public class Card : TemplatedView, IBackgroundElement, IBorderElement, ICommandElement
{
    public static readonly BindableProperty ContentProperty = IContentElement.ContentProperty;

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty CornerRadiusShapeProperty = ICornerRadiusShapeElement.CornerRadiusShapeProperty;

    [TypeConverter(typeof(CornerRadiusShapeConverter))]
    public CornerRadiusShape CornerRadiusShape { get => (CornerRadiusShape)GetValue(CornerRadiusShapeProperty); set => SetValue(CornerRadiusShapeProperty, value); }

    public ElementState ViewState { get; set; }
    public bool Enable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Color BorderColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int BorderThickness { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ICommand Command { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public object CommandParameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void OnPropertyChanged()
    {
        InvalidateMeasure();
    }
}