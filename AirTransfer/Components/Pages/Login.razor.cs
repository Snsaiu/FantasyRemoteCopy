using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Login : ComponentBase
{
    [Parameter] public string? LoginName { get; set; }

    [Parameter] public string? DeviceName { get; set; }


    [Inject] private NavigationManager NavigationManager { get; set; }

    private Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(DeviceName))
        {
            return Task.CompletedTask;
        }

        this.NavigationManager.NavigateTo($"/home");

        return Task.CompletedTask;
    }
}