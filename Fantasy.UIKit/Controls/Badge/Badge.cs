namespace Fantasy.UIKit
{
    [ContentProperty(nameof(Content))]
    public partial class Badge : TemplatedView, IForegroundElement, IContentElement, IVisualTreeElement,
        IBackgroundElement, ITextElement, IFontElement
    {
        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;
        public View Content { get; set; }
        public ElementState ViewState { get; set; }
        public bool Enable { get; set; }

        void IUIKitElement.OnPropertyChanged()
        {
            throw new NotImplementedException();
        }

        public CornerRadiusShape CornerRadiusShape { get; set; }
        public string Text { get; set; }
        public float FontSize { get; set; }
        public string FontFamily { get; set; }
        public FontWeight FontWeight { get; set; }
        public bool FontIsItalic { get; set; }
    }
}