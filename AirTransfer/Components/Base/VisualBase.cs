using AirTransfer.Interfaces;
using AirTransfer.Language;

using Microsoft.FluentUI.AspNetCore.Components;

namespace Microsoft.AspNetCore.Components;

public abstract class VisualBase : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected IStateManager StateManager { get; set; } = null!;

    [Inject] protected IToastService ToastService { get; set; } = null!;

    [Inject] protected ISaveDataService SaveDataService { get; set; } = null!;

    protected LocalizationResourceManager Localizer => LocalizationResourceManager.Instance;
    [Inject] protected ISystemType SystemType { get; set; } = null!;

    public bool IsBusy { get; set; }
}