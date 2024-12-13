using AirTransfer.Interfaces;

using FantasyRemoteCopy.UI.Consts;

using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IStateManager StateManager { get; set; } = null!;

    [Parameter] public string Text { get; set; }


    protected override Task OnInitializedAsync()
    {
        if (StateManager.ExistKey(ConstParams.StateManagerKeys.ListenKey))
        {
            var state = StateManager.GetState<bool>(ConstParams.StateManagerKeys.ListenKey);
        }
        else
        {
            StateManager.SetState(ConstParams.StateManagerKeys.ListenKey, true);
        }
        return base.OnInitializedAsync();

    }

    private void GotoTextInputPage()
    {
        NavigationManager.NavigateTo("/Home/TextInput");
    }
}