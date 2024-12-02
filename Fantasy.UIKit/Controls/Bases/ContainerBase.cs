using Microsoft.Maui.Converters;

using System.ComponentModel;

namespace Fantasy.UIKit.Controls.Bases;

public abstract class ContainerBase : TemplatedView, IContentElement,
    IBackgroundElement, IBorderElement
{
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public static readonly BindableProperty BackgroundColorProperty = IBackgroundElement.BackgroundColorProperty;


    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty ContentProperty = IContentElement.ContentProperty;

    public ElementState ViewState { get; set; } = ElementState.Normal;

    public bool Enable
    {
        get => (bool)GetValue(EnableProperty);
        set => SetValue(EnableProperty, value);
    }

    void IUIKitElement.OnPropertyChanged()
    {
        OnPropertyChanged();
    }

    protected abstract void OnPropertyChanged();

    public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

    [TypeConverter(typeof(CornerRadiusShapeConverter))]
    public CornerRadiusShape CornerRadiusShape
    {
        get => (CornerRadiusShape)GetValue(ICornerRadiusShapeElement.CornerRadiusShapeProperty);
        set => SetValue(ICornerRadiusShapeElement.CornerRadiusShapeProperty, value);
    }

    public static readonly BindableProperty ColorProperty = IBorderElement.BorderColorProperty;

    public Color BorderColor
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty BorderThicknessProperty = IBorderElement.BorderThicknessProperty;


    public int BorderThickness
    {
        get => (int)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }
}