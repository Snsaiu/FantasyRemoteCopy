using System.ComponentModel;

namespace Fantasy.UIKit
{
    [ContentProperty(nameof(Content))]
    public class Badge : TemplatedView, IContentElement,
        IBackgroundElement, ITextElement, IFontElement
    {
        private AbsoluteLayout PART_Root;

        private BadgeContainer PART_Container;

        public Badge()
        {
            SetDynamicResource(StyleProperty, "DefaultBadgeStyle");
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Root = (AbsoluteLayout)GetTemplateChild(nameof(PART_Root));
            PART_Container = (BadgeContainer)GetTemplateChild(nameof(PART_Container));
            OnChildAdded(PART_Root);
            VisualDiagnostics.OnChildAdded(this, PART_Root);
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BackgroundColorProperty = IBackgroundElement.BackgroundColorProperty;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetInheritedBindingContext(PART_Root, BindingContext);
        }

        public IReadOnlyList<IVisualTreeElement> GetVisualChildtren()
        {
            return PART_Root is null ? Array.Empty<IVisualTreeElement>() : [PART_Root];
        }

        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        public static readonly BindableProperty ForegroundColorProperty = IForegroundElement.ForegroundColorProperty;

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

        public static readonly BindableProperty EnableProperty = IUIKitElement.EnableProperty;

        void IUIKitElement.OnPropertyChanged()
        {
            var size = this.GetStringSize();
            AbsoluteLayout.SetLayoutBounds(PART_Container,
                new Rect(1, 0, Math.Max(20, size.Width), Math.Max(10, size.Height)));
            InvalidateMeasure();
        }

        [TypeConverter(typeof(CornerRadiusShapeConverter))]
        public CornerRadiusShape CornerRadiusShape
        {
            get => (CornerRadiusShape)GetValue(ICornerRadiusShapeElement.CornerRadiusShapeProperty);
            set => SetValue(ICornerRadiusShapeElement.CornerRadiusShapeProperty, value);
        }

        public static readonly BindableProperty TextProperty = ITextElement.TextProperty;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;

        public float FontSize
        {
            get => (float)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;

        public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        public static readonly BindableProperty FontIsItalicProperty = IFontElement.FontIsItalicProperty;

        public bool FontIsItalic
        {
            get => (bool)GetValue(FontIsItalicProperty);
            set => SetValue(FontIsItalicProperty, value);
        }
    }
}