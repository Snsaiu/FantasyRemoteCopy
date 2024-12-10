using AirTransfer.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Setting : ComponentBase
{
    #region Injects

    [Inject] private IUserService UserService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region Parameters

    #endregion

    #region Commands

    private async Task LogoutCommand()
    {
        await UserService.ClearUserAsync();
        NavigationManager.NavigateTo("/");
    }

    #endregion
}