namespace Fantasy.UIKit.Interfaces
{
    internal interface IUIKitElement
    {
        public bool Enable { get; set; }

        public static readonly BindableProperty EnableProperty = BindableProperty.Create(nameof(Enable), typeof(bool), typeof(IUIKitElement));
    }
}
