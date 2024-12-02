using Fantasy.UIKit.Controls.Bases;

using System.ComponentModel;

namespace Fantasy.UIKit
{
    [ContentProperty(nameof(Content))]
    public class Badge : ContainerBase, ITextElement, IFontElement
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


        protected override void OnPropertyChanged()
        {
            var size = this.GetStringSize();


            AbsoluteLayout.SetLayoutBounds(PART_Container,
                new Rect(1.05, -0.1, size.Width + 5, size.Height + 5));

            InvalidateMeasure();
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