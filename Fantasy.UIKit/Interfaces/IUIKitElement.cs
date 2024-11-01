#region

using Fantasy.UIKit.Primitives;

#endregion

namespace Fantasy.UIKit.Interfaces;

internal interface IUIKitElement
{
    public static readonly BindableProperty EnableProperty =
        BindableProperty.Create(nameof(Enable), typeof(bool), typeof(IUIKitElement));

    public ElementState ViewState { get; set; }

    public bool Enable { get; set; }

    void OnPropertyChanged();

    /// <summary>
    ///     重新测量
    /// </summary>
    void InvalidateMeasure()
    {
        OnPropertyChanged();
    }
}